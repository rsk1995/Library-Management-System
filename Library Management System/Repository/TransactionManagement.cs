using Library_Management_System.Migrations;
using System.Net;
using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Repository
{
    public class TransactionManagement:ITransactionsManagement
    {
        private readonly LMSDbContext _context;
        public TransactionManagement(LMSDbContext context)
        {
            _context = context;
        }
        public async Task<Books> BorrowBook(Users user, Books book)
        {
           await _context.Transactions.AddAsync(new Transactions
            {
                UserId = user.UserId,
                BookId = book.BookId,
                BorrowDate = DateTime.Now
            });
            book.Status = "Checked Out";
           await _context.SaveChangesAsync();
            return book;
        }
        
    }
}
