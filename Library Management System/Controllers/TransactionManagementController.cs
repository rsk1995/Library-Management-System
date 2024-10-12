using Library_Management_System.Models;
using Library_Management_System.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Library_Management_System.Controllers
{
   // [Authorize(Roles = "Librarian")]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionManagementController : ControllerBase
    {
        private readonly LMSDbContext _context;
        private readonly ITransactionsManagement _transactionManagement;
        private readonly IBookManagement _bookManagement;
        private readonly IUserManagement _userManagement;
        public TransactionManagementController(LMSDbContext context,ITransactionsManagement transactionsManagement,IBookManagement bookManagement,IUserManagement userManagement)
        {
            _context = context;
            _transactionManagement = transactionsManagement;
            _bookManagement = bookManagement;
            _userManagement = userManagement;
        }

        [HttpPost]
        [Route("BorrowBook")]
        public async Task<IActionResult> BorrowBook(int userid, int bookid)
        {
            var exuser = await _userManagement.GetUserById(userid);
            var exbook = await _bookManagement.GetBookById(bookid);
             
            if (exuser.IsActive == 1 && exbook.Status=="Available")
            {
                var borrow = await _transactionManagement.BorrowBook(exuser,exbook);
                return Ok("Book borrowed Successfully");
            }
            else
            {
                return BadRequest("User deactivated or book not available!");
            }
        }

        [HttpPut]
        [Route("ReturnBook")]
        public async Task<IActionResult> ReturnBook(int tid)
        {
            var extran = await _transactionManagement.GetTransactionById(tid);
            if (extran == null)
            {
                return NotFound("Incorrect transaction ID!");
            }
            else
            {
                var exbook = await _bookManagement.GetBookById(extran.BookId);
                var trans = await _transactionManagement.ReturnBook(extran, exbook);
                if (trans.FineAmount == 0)
                {
                    return Ok("Book returned without fine!");
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

        [HttpGet]
        [Route("GetTransationById")]

        public async Task<ActionResult<Transactions>> GetTransationById(int tid)
        {
            var trans = await _transactionManagement.GetTransactionById(tid);
            if (trans == null)
            {
                return NotFound("Transaction Not Found");
            }
            else
            {
                return Ok(trans);
            }
        }
    }
}
