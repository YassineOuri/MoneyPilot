using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        private readonly ApplicationDbContext _context;
        private readonly PasswordService _passwordService;

        public AuthController(ApplicationDbContext context, PasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
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

            if(!_passwordService.ValidatePassword(user.Password, existingUser.Password))
            {
                return BadRequest("Credentials are invalid");
            }

            var token = GenerateJwtToken(user.Email);
            var loggedInUser = new
            {
                existingUser.FirstName,
                existingUser.LastName,
                existingUser.Email
            };
            return Ok(new { loggedInUser , token });
        }



        [HttpPost("register")]
        public async Task<IActionResult> register(UserRegister user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (user == null)
            {
                return BadRequest("Please submit the user to register");
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
            if(existingUser != null)
            {
                return BadRequest("User with this email already exists");
            }


            var newUser = new User
            (
                user.FirstName,
                user.LastName,
                user.Email,
                _passwordService.HashPassword(user.Password)
            );

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(register), "User registered successfully" , newUser);


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
