using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace Library_Management_System.Repository
{
    public interface ITransactionsManagement
    {
        Task<Books> BorrowBook(Users user, Books book);
        Task<Transactions> ReturnBook(Transactions extran, Books exbook);
        Task<Transactions> GetTransactionById(int id);
        Task ReserveBook(Users user, Books book);
        Task BorrowReserveBook(Transactions transactions, Books book);
        Task<IEnumerable<Transactions>> GetReservedTransaction(int uid);
    }
}
