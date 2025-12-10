using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using MoneyPilot.Data;
using MoneyPilot.DTO;
using MoneyPilot.Models;

namespace MoneyPilot.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetCategories()
        {
            List<Category> categories = await _context.Categories
                .Where(c => c.IsVisible)
                .ToListAsync();
            return Ok(categories);
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories
                .Include(c => c.SubCategories)
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound("Category not found");
            }

            return Ok(category);
        }

        [Authorize]
        [HttpGet("sub/{parentId:int}")]
        public async Task<ActionResult<List<Category>>> GetSubCategories(int parentId)
        {
            var parentCategory = await _context.Categories.FindAsync(parentId);

            if (parentCategory == null)
            {
                return NotFound("Parent category not found");
            }

            var subCategories = await _context.Categories
                .Where(c => c.ParentCategoryId == parentId && c.IsVisible)
                .Select(c => new { 
                    c.Name,
                    c.CategoryNature,
                    c.Icon
                })
                .ToListAsync();

            return Ok(subCategories);
        }

        [Authorize]
        [HttpGet("root")]
        public async Task<ActionResult<List<Category>>> GetRootCategories()
        {
            var rootCategories = await _context.Categories
                .Where(c => c.ParentCategoryId == null && c.IsVisible)
                .Include(c => c.SubCategories)
                .ToListAsync();

            return Ok(rootCategories);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDTO category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var newCategory = new Category(
                category.Name,
                category.Icon,
                category.CategoryNature,
                category.IsVisible
                
               
            );

            newCategory.ParentCategoryId = category.ParentCategoryId;
               

            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(AddCategory), new { id = newCategory.Id }, newCategory);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryDTO categoryToUpdate, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCategory = await _context.Categories.FindAsync(id);
            if (existingCategory == null)
            {
                return NotFound("Category not found");
            }

            var parentCategory = await _context.Categories.FindAsync(categoryToUpdate.ParentCategoryId);
            if (parentCategory == null)
            {
                return NotFound("Parent category not found");
            }

            if (categoryToUpdate.ParentCategoryId == id)
            {
                return BadRequest("Category cannot be its own parent");
            }

            ICollection<Category> subCategories = categoryToUpdate.SubCategories ?? [];

            if(subCategories.Count > 0)
            {
                var subCategoriesIds = subCategories.Select(s => s.Id).ToList();
                if(subCategoriesIds.Contains(categoryToUpdate.Id))
                {
                    return BadRequest("Category's Parent cannot be one of its children");
                }
            } 


            existingCategory.Name = categoryToUpdate.Name;
            existingCategory.Icon = categoryToUpdate.Icon;
            existingCategory.CategoryNature = categoryToUpdate.CategoryNature;
            existingCategory.IsVisible = categoryToUpdate.IsVisible;
            existingCategory.ParentCategoryId = categoryToUpdate.ParentCategoryId;

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(UpdateCategory), new { id = existingCategory.Id }, existingCategory);
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories
                .Include(c => c.SubCategories)
                .Include(c => c.Transactions)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound("Category not found");
            }

            if (category.SubCategories != null && category.SubCategories.Any())
            {
                return BadRequest("Cannot delete category with subcategories. Please delete or move subcategories first.");
            }

            if (category.Transactions != null && category.Transactions.Any())
            {
                return BadRequest("Cannot delete category that is used in transactions.");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok($"Category of Id {category.Id} deleted successfully");
        }

        /** private async Task<bool> IsCircularReference(int categoryId, int potentialParentId)
        {
            var currentParentId = potentialParentId;
            var visited = new HashSet<int> { categoryId };

            while (currentParentId != categoryId)
            {
                if (visited.Contains(currentParentId))
                {
                    return true;
                }

                visited.Add(currentParentId);

                var parent = await _context.Categories.FindAsync(currentParentId);
                if (parent == null || parent.ParentCategoryId == parent.Id)
                {
                    break;
                }

                currentParentId = parent.ParentCategoryId;
            }

            return currentParentId == categoryId;
        }**/
    }
}

