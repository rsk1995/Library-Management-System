
using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role{get;set;}
        public DateTime MembershipDate { get; set; } = DateTime.Now;
        public int IsActive { get; set; } = 1;


    }
}
