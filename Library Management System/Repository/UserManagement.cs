using Library_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Repository
{
    public class UserManagement:IUserManagement
    {
        private readonly LMSDbContext _context;
        //private readonly IUserManagement _userManagement;
        public UserManagement(LMSDbContext context)
        {
            _context = context;
            //_userManagement = userManagement;
        }

        public async Task<IEnumerable<Users>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<UserDTO> AddUser(UserDTO user)
        {
            
               await _context.Users.AddAsync(new Users
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = user.Password,
                    Role = user.Role
                });
               await _context.SaveChangesAsync();
                return user;
            
            
        }


        public async Task<IEnumerable<Users>> GetUserByRole(string role)
        {
            return await _context.Users.Where(p => p.Role.Contains(role)).ToListAsync();
        }

        public async Task<Users> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<Users> DeactivateUser(int id)
        { 
            var exuser=await _context.Users.FindAsync(id);
        
             exuser.IsActive = 0;
             await _context.SaveChangesAsync();
            return exuser;
        }

        public async Task<Users> ReactivateUser(int id)
        {
            var exuser = await _context.Users.FindAsync(id);
            exuser.IsActive = 1;
            await _context.SaveChangesAsync();
            return exuser;
        }

        public async Task<Users> UpdateUserInfomation(UpdateUser user)
        {
            var exuser = await _context.Users.FindAsync(user.UserId);
            exuser.FirstName = user.FirstName;
            exuser.LastName = user.LastName;
            exuser.Role = user.Role;
            await _context.SaveChangesAsync();
            return exuser;
        }

        public async Task<Users> DeleteUser(int uid)
        {
            var exuser = await _context.Users.FindAsync(uid);
            _context.Users.Remove(exuser);
            await _context.SaveChangesAsync();
            return exuser;
        }

    }
}
