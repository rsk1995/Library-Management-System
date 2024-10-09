using Library_Management_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Library_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionManagementController : ControllerBase
    {
        private readonly LMSDbContext _context;
        public TransactionManagementController(LMSDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("BorrowBook")]
        public IActionResult BorrowBook(int userid, int bookid)
        {
            var exuser = _context.Users.FirstOrDefault(p => p.UserId == userid);
            var exbook = _context.Books.FirstOrDefault(p => p.BookId == bookid);
            if (exuser.IsActive == 1 && exbook.Status=="Available")
            {
                _context.Transactions.Add(new Transactions
                {
                    UserId = userid,
                    BookId = bookid,
                    BorrowDate=DateTime.Now
                });
                exbook.Status = "Checked Out";
                _context.SaveChanges();
                return Ok("Book borrowed Successfully");
            }
            else
            {
                return BadRequest("User deactivated or book not available!");
            }
        }

        [HttpPut]
        [Route("ReturnBook")]
        public IActionResult ReturnBook(int tid)
        {
            var extran = _context.Transactions.FirstOrDefault(p => p.TransactionsID == tid);
            if (extran == null)
            {
                return NotFound("Incorrect transaction ID!");
            }
            else
            {
                DateTime borrowdate = extran.BorrowDate;
                DateTime returndate = DateTime.Now;
                int days = Convert.ToInt32(returndate - borrowdate);
                if (days < 7)
                {
                    extran.ReturnDate = DateTime.Now;
                    extran.FineAmount = 0;
                    _context.SaveChanges();
                    return Ok("Book returned without fine!");
                }
                else if (days > 7)
                {
                    extran.ReturnDate = DateTime.Now;
                    extran.FineAmount = days * 10;
                    _context.SaveChanges();
                }
                return Ok("Book returned with fine!");
            }
        }

        [HttpPut]
        [Route("ReserveBook")]
        public IActionResult ReserverBook(int bid)
        {
            var exbook = _context.Books.FirstOrDefault(p => p.BookId == bid);
            if (exbook == null)
            {
                return NotFound("Book not found!");
            }
            else
            {
                exbook.Status = "Reserved";
                _context.SaveChanges();
                return Ok("Book reserved!");
            }
        }


    }
}
