using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using auth.Models;
using auth.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _dbContext;

        public AuthController(IConfiguration configuration, AppDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            var user = await _dbContext.Users
                .Include(u => u.emp)
                .Include(u => u.role)
                .SingleOrDefaultAsync(u => u.matricule == login.matricule && u.password == login.password);

            if (user != null)
            {
                if (user?.emp == null)
                {
                    return BadRequest("Employee information is missing");
                }


                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.matricule.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.user_id.ToString()),
                    new Claim(ClaimTypes.GroupSid, user.emp_id.ToString()),
                    new Claim("emp_id", user.emp.emp_id.ToString()), 
                    new Claim("emp_nom", user.emp.emp_nom), 
                    new Claim("emp_prenom", user.emp.emp_prenom),
                    new Claim(ClaimTypes.Role, user.role.role_id.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                // Build the response with the token and user information
                return Ok(new 
                { 
                    Token = tokenString,
                    User = new 
                    {
                        emp_id = user.emp_id,
                        emp_nom = user.emp.emp_nom,
                        emp_prenom = user.emp.emp_prenom,
                        role_id = user.role.role_id,
                    }
                });
            }
            else
            {
                return Unauthorized("Matricule ou mot de passe incorrect");
            }
        }
    }
}