using Library_Management_System.Models;

namespace Library_Management_System.Repository
{
    public interface IUserManagement
    {
        Task<UserDTO> AddUser(UserDTO user);
        Task<IEnumerable<Users>> GetAllUsers();
        Task<IEnumerable<Users>> GetUserByRole(string role);
        Task<Users> GetUserById(int id);
        Task<Users> DeactivateUser(int id);
        Task<Users> ReactivateUser(int id);
        Task<Users> UpdateUserInfomation(UpdateUser user);
        Task<Users> DeleteUser(int uid);
    }
}
