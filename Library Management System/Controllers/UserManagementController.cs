using Library_Management_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly LMSDbContext _context;
        public UserManagementController(LMSDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("AddUser")]

        public IActionResult AddNewUser([FromBody] UserDTO user)
        {
            var exuser = _context.Users.FirstOrDefault(p => p.Email == user.Email);
            if (exuser == null)
            {
                _context.Users.Add(new Users
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = user.Password,
                    Role = user.Role
                });
                _context.SaveChanges();
                return Ok("New User Added Successfully...!");
            }
            else
            {
                return Ok("User Already Exist");
            }
        }

        [HttpGet]
        [Route("GetAllUser")]
        public IActionResult GetAllUser()
        {
            return Ok(_context.Users.ToList());
        }

        [HttpGet]
        [Route("GetUserById")]

        public IActionResult GetUserById(int id)
        {
            var exuser = _context.Users.FirstOrDefault(p => p.UserId == id);

            if (exuser == null)
            {
                return NotFound("User Not Found");
            }
            else
            {
                return Ok(exuser);
            }
        }
        [HttpGet]
        [Route("GetUserByRole")]

        public IActionResult GetUserByRole(string role)
        {
            var exusers = _context.Users.Where(p => p.Role == role).ToList();

            if (exusers.Any())
            {
                return Ok(exusers);

            }
            else
            {
                return NotFound("Users Not Found");
            }
        }
        [HttpPut]
        [Route("DeactiveUser")]
        public IActionResult DeactivateUser(int id)
        {
            var exuser = _context.Users.FirstOrDefault(p => p.UserId == id);

            if (exuser == null)
            {
                return NotFound("User Not Found");
            }
            else if(exuser.IsActive==0)
            {
                return BadRequest("User Already Deactivated");
            }
            else
            {
                exuser.IsActive = 0;
                _context.SaveChanges();
                return Ok("User " + exuser.FirstName + " " + exuser.LastName + " has been deactivated.");
            }
        }


        [HttpPut]
        [Route("ReactiveUser")]
         public IActionResult ReactivateUser(int id)
         {
            var exuser = _context.Users.FirstOrDefault(p => p.UserId == id);
            if (exuser == null)
            {
                return NotFound("User Not Found");
            }
            else if (exuser.IsActive==1)
            {
                return BadRequest("User Already Activated");
            }
            else
            {
                exuser.IsActive = 1;
                _context.SaveChanges();
                return Ok("User " + exuser.FirstName + " " + exuser.LastName + " has been reactivated.");
            }
         }
    } 
}
