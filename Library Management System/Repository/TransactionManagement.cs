using Library_Management_System.Migrations;
using System.Net;
using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Transactions> ReturnBook(Transactions extran, Books exbook)
        {
            DateTime borrowdate = extran.BorrowDate;
            DateTime returndate = DateTime.Now;
            TimeSpan diff = returndate - borrowdate;
            int days = (int)diff.TotalDays;
            if (days < 7)
            {
                extran.ReturnDate = DateTime.Now;
                extran.FineAmount = 0;
                exbook.Status = "Available";
                await _context.SaveChangesAsync();
                return extran;
            }
            else  
            {
                extran.ReturnDate = DateTime.Now;
                extran.FineAmount = days * 10;
                exbook.Status = "Available";
                await _context.SaveChangesAsync();
                return extran;
            }
        }

        public async Task<Transactions> GetTransactionById(int id)
        {
            return await _context.Transactions.FindAsync(id);
        }

        public async Task ReserveBook(Users user, Books book)
        {
            await _context.Transactions.AddAsync(new Transactions
            {
                UserId = user.UserId,
                BookId = book.BookId
            });
            book.Status = "Reserved";
            await _context.SaveChangesAsync();
        }

        public async Task BorrowReserveBook(Transactions transactions, Books book)
        {
            book.Status = "Checked Out";
            transactions.BorrowDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Transactions>> GetReservedTransaction(int uid)
        {
            var extrans = await _context.Transactions.Where(p => p.UserId == uid).ToListAsync();
            return extrans;
        }
    }
}
