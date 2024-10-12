using Library_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Repository
{
    public class UserManagement:IUserManagement
    {
        private readonly LMSDbContext _context;
        public UserManagement(LMSDbContext context)
        {
            _context = context;
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
             _context.SaveChangesAsync();
            return exuser;
        }

        public async Task<Users> ReactivateUser(int id)
        {
            var exuser = await _context.Users.FindAsync(id);

            exuser.IsActive = 1;
            _context.SaveChangesAsync();
            return exuser;
        }


    }
}
