using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Repository
{
    public interface ITransactionsManagement
    {
        Task<Books> BorrowBook(Users user, Books book);
    }
}
