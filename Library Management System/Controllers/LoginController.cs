using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Library_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Library_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly LMSDbContext _context;
        public LoginController(IConfiguration configuration, LMSDbContext context)
        {
            _configuration= configuration;
            _context= context;

        }


        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] LoginDTO loguser)
        {
            var exuser = _context.Users.FirstOrDefault(option => option.Email == loguser.Email && option.Password== loguser.Password);

            if (exuser != null)
            {
                var claims = new[]
                {
                     new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"]),
                     new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),

                     new Claim(ClaimTypes.NameIdentifier, exuser.UserId.ToString()),
                     //new Claim("EmployeeID",employee.EmployeeID.ToString()),
                     //new Claim("Role",employee.Role.ToString())
                     new Claim(ClaimTypes.Role, exuser.Role)
                     //new Claim("Email",exuser.Email.ToString()),
                     //new Claim("Password",exuser.Password.ToString()),
                     //new Claim("Role",exuser.Role.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(

                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(60),
                    signingCredentials: signIn
                    );
                string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new { Token = tokenValue, UserInfo = exuser });
            }
            else
            {
                return Unauthorized();
            }

        }

    }
}
