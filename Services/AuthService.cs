using Microsoft.EntityFrameworkCore;
using MoneyPilot.Data;
using MoneyPilot.Models;
using System.Security.Claims;

namespace MoneyPilot.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> getCurrentUserID(ClaimsPrincipal User)
        {
            ClaimsPrincipal currentUser = User;
            var ownerEmail = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(ownerEmail))
            {
                throw new UnauthorizedAccessException("Invalid token: User email not found in token");
            }

            var loggedInUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == ownerEmail);

            return loggedInUser!.Id ;

        }
    }
}
