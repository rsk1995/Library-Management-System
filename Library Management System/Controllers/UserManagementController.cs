using Library_Management_System.Models;
using Library_Management_System.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly LMSDbContext _context;
        private readonly IUserManagement _userManagement;
        public UserManagementController(LMSDbContext context, IUserManagement userManagement)
        {
            _context = context;
            _userManagement = userManagement;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddNewUser([FromBody] UserDTO user)
        {
            var exuser = _context.Users.FirstOrDefault(p => p.Email==user.Email);
                if (exuser==null)
                {


                    var createdUser = await _userManagement.AddUser(user);
                    return Ok("User added successfully");
                }
                else
                {
                    return Ok("User Already Exist");
                }
            

            
        }

        [Authorize(Roles = "Admin,Librarian")]
        [HttpGet]
        [Route("GetAllUser")]
        public async Task<ActionResult<IEnumerable<Books>>> GetAllUser()
        {
            var AllUsers = await _userManagement.GetAllUsers();
            return Ok(AllUsers);
        }


        [Authorize(Roles = "Admin,Librarian,Member")]
        [HttpGet]
        [Route("GetUserById")]
        public async Task<ActionResult<Users>> GetUserById(int id)
        {
            
            var exuser = await _userManagement.GetUserById(id); 
            if (exuser == null)
            {
                return NotFound("User Not Found");
            }
            else
            {
                return Ok(exuser);
            }
        }

        [Authorize(Roles = "Admin,Librarian")]
        [HttpGet]
        [Route("GetUserByRole")]

        public async Task<ActionResult<Users>> GetUserByRole(string role)
        {
            var exusers = await _userManagement.GetUserByRole(role);

            if (exusers.Any())
            {
                return Ok(exusers);

            }
            else
            {
                return NotFound("Users Not Found");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("DeactiveUser")]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            var exuser = await _userManagement.GetUserById(id);

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
                var exuser1 = await _userManagement.DeactivateUser(id);
                return Ok("User " + exuser1.FirstName + " " + exuser1.LastName + " has been deactivated.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("ReactiveUser")]
         public async Task<IActionResult> ReactivateUser(int id)
         {
            var exuser = await _userManagement.GetUserById(id);
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
                var exuser1 = await _userManagement.ReactivateUser(id);
                return Ok("User " + exuser.FirstName + " " + exuser.LastName + " has been reactivated.");
            }
         }

        [Authorize(Roles = "Admin,Librarian")]
        [HttpPut]
        [Route("UpdateUserInfo")]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateUser user)
        {
            var exuser = await _userManagement.GetUserById(user.UserId);
            if (exuser == null)
            {
                return NotFound("User Not Found!");
            }
            else
            {
                var user1 = await _userManagement.UpdateUserInfomation(user);
                return Ok("User information updated successfully!");
            }
        }

        [Authorize(Roles = "Admin,Librarian")]
        [HttpDelete]
        [Route("DeleteUser")]
        public async Task<ActionResult<Users>> DeleteUser(int uid)
        {

            var exuser = await _userManagement.GetUserById(uid);
            if (exuser == null)
            {
                return NotFound("User Not Found");
            }
            else
            {
                var user = await _userManagement.DeleteUser(uid);
                return Ok("User Deleted Successfully!\n\n" + user.ToString());
            }
        }
    } 
}