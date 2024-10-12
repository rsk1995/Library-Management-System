using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Repository
{
    public interface ITransactionsManagement
    {
        Task<Books> BorrowBook(Users user, Books book);
        Task<Transactions> ReturnBook(Transactions extran, Books exbook);
         Task<Transactions> GetBookById(int id);
    }
}
