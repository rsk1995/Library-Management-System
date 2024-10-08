using Library_Management_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksManagementController : ControllerBase
    {
        private readonly LMSDbContext _context;
        public BooksManagementController(LMSDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("AddBooks")]

        public IActionResult AddNewBook([FromBody] BookDTO book)
        {
            _context.Books.Add(new Books
            {
                Title = book.Title,
                Author = book.Author,
                Generation = book.Generation,
                ISBN = book.ISBN,
                PublicationYear= book.PublicationYear,
                Status=book.Status
            });
            _context.SaveChanges();
            return Ok("Book Added Successfully");

        }

        [HttpGet]
        [Route("GetAllBooks")]
        public IActionResult GetAllUser()
        {
            return Ok(_context.Books.ToList());
        }

        [HttpGet]
        [Route("GetUserById")]

        public IActionResult GetBookById(int id)
        {
            var book = _context.Books.FirstOrDefault(p => p.BookId == id);

            if (book == null)
            {
                return NotFound("Book Not Found");
            }
            else
            {
                return Ok(book);
            }
        }
        [HttpGet]
        [Route("GetBookByTitle")]

        public IActionResult GetBookByTitle(string title)
        {
            var Btitle = _context.Books.Where(p => p.Title == title).ToList();

            if (Btitle.Any())
            {
                return Ok(Btitle);

            }
            else
            {
                return NotFound("Books Not Found");
            }
        }
        [HttpGet]
        [Route("GetBookByAuthor")]

        public IActionResult GetBookByAuthor(string author)
        {
            var Bauthor = _context.Books.Where(p => p.Author == author).ToList();

            if (Bauthor.Any())
            {
                return Ok(Bauthor);

            }
            else
            {
                return NotFound("Books Not Found");
            }
        }




    }
    }

