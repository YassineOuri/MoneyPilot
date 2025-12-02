using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using MoneyPilot.Data;
using MoneyPilot.DTO;
using MoneyPilot.Models;
using MoneyPilot.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MoneyPilot.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private static ApplicationDbContext _context;
        private static BcryptService _bcryptService;

        public AuthController(ApplicationDbContext context, BcryptService bcryptService)
        {
            _context = context;
            _bcryptService = bcryptService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] UserLoginDTO user)
        {   
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser =  await _context.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
            if(existingUser == null)
            {
                return NotFound();
            }

            if(!_bcryptService.ValidatePassword(user.Password, existingUser.Password))
            {
                return BadRequest("credentials are invalid");
            }

            var token = GenerateJwtToken(user.Email);
            return Ok(new { user, token });
        }



        [HttpPost("register")]
        public async Task<IActionResult> register(UserRegister user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(user == null)
            {
                return BadRequest("Please submit the user to register");

            }

            user.Password = _bcryptService.HashPassword(user.Password);
            _context.Users.Add((User)(user!));
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(register), new { email = user.Email }, user);


        }

        private string GenerateJwtToken(string email)
        {
            DotNetEnv.Env.Load();
            var claims = new[]
            {
               new Claim(JwtRegisteredClaimNames.Sub, email),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
            issuer: "moneypilot.com",
            audience: "moneypilot.com",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
