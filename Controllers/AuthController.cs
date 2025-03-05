using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BCrypt.Net;
using EHRsafe_Task.DTOs;


namespace EHRsafe_Task.Controllers
{
	[Route("api/auth")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly AppDbContext _context;

		public AuthController(AppDbContext context)
		{
			_context = context;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(UserRegisterDto request)
		{
			var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
			if (existingUser != null)
				return BadRequest("User already exists");

			var user = new User
			{
				FirstName = request.FirstName,
				LastName = request.LastName,
				Email = request.Email,
				PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)  // Secure Password
			};

			_context.Users.Add(user);
			await _context.SaveChangesAsync();
			return Ok("User registered successfully");
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(UserLoginDto request)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
			if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
				return BadRequest("Invalid credentials");

			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes("YourVeryStrongSuperSecretKeyWithMoreCharacters!");

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) }),
				Expires = DateTime.UtcNow.AddHours(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return Ok(new { Token = tokenHandler.WriteToken(token) });
		}


	}
}
