﻿using Library_Management_System.Models;
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
        public IActionResult ReturnBook(int tid)
        {
            var extran = _context.Transactions.FirstOrDefault(p => p.TransactionsID == tid);
            var exbook = _context.Books.FirstOrDefault(p => p.BookId == extran.BookId);
            if (extran == null)
            {
                return NotFound("Incorrect transaction ID!");
            }
            else
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
                    _context.SaveChanges();
                    return Ok("Book returned without fine!");
                }
                else if (days > 7)
                {
                    extran.ReturnDate = DateTime.Now;
                    extran.FineAmount = days * 10;
                    exbook.Status = "Available";
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
