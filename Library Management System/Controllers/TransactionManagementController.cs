using Library_Management_System.Migrations;
using Library_Management_System.Models;
using Library_Management_System.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Cryptography;

namespace Library_Management_System.Controllers
{
    //[Authorize(Roles = "Librarian")]
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
        
        [Authorize(Roles = "Admin,Librarian")]
        [HttpPost]
        [Route("BorrowBook")]
        public async Task<IActionResult> BorrowBook(int userid, int bookid)
        {
            var exuser = await _userManagement.GetUserById(userid);
            var exbook = await _bookManagement.GetBookById(bookid);
            if (exbook == null || exuser == null)
            {
                return NotFound("User or Book not found!");
            }
            else if (exuser.IsActive == 1 && exbook.Status=="Available")
            {
                var borrow = await _transactionManagement.BorrowBook(exuser,exbook);
                return Ok("Book borrowed Successfully");
            }
            else
            {
                return BadRequest("User deactivated or book not available!");
            }
        }

        //[Authorize(Roles = "Admin,Librarian")]
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
        
        //[Authorize(Roles = "Admin,Librarian,Member")]
        [HttpPost]
        [Route("ReserveBook")]
        public async Task<IActionResult> ReserverBook(int uid, int bid)
        {

            var exuser = await _userManagement.GetUserById(uid);
            var exbook = await _bookManagement.GetBookById(bid);
            if (exbook == null || exuser == null)
            {
                return NotFound("User or Book not found!");
            }
            else if (exuser.IsActive == 1 && exbook.Status == "Available")
            {
                await _transactionManagement.ReserveBook(exuser, exbook);
                return Ok("Book reserved Successfully");
            }
            else
            {
                return BadRequest("User deactivated or book not available!");
            }
        }

        //[Authorize(Roles = "Admin,Librarian")]
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

        //[Authorize("Admin,Librarian")]
        [HttpPatch]
        [Route("BorrowReservedBooks")]
        public async Task<ActionResult> BorrowResevedBooks(int tid)
        {
            var trans = await _transactionManagement.GetTransactionById(tid);
            if (trans == null)
            {
                return NotFound("Transaction Not Found");
            }
            else
            {
                var exbook = await _bookManagement.GetBookById(trans.BookId);
                await _transactionManagement.BorrowReserveBook(trans,exbook);
                return Ok("Reserved book borrowed Successfully");
            }
        }

        //[Authorize("Admin,Librarian")]
        [HttpGet]
        [Route("GetReservedTransaction")]
        public async Task<ActionResult<Transactions>> GetReservedTransaction(int uid)
        {
            var exuser = await _userManagement.GetUserById(uid);
            if (exuser == null)
            {
                return NotFound("User not found!");
            }
            else if (exuser.IsActive == 1)
            {
                var extrans = await _transactionManagement.GetReservedTransaction(uid);
                if (extrans.Any())
                {
                    return Ok(extrans);
                }
                else
                {
                    return NotFound("Transaction not found!");
                }
            }
            else
            {
                return BadRequest("User deactivated!");
            }
        }
    }
}
