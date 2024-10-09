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
        [Route("GetBookById")]

        public IActionResult GetBookById(int id)
        {
            var book = _context.Books.FirstOrDefault(p => p.BookId == id);

            if (book == null)
            {
                return NotFound("Book Not Found For Requested ID");
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
                return NotFound("Books Not Found For Requested Title");
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
                return NotFound("Books Not Found For Requested Author");
            }
        }

        [HttpGet]
        [Route("GetBookByPublicationYear")]

        public IActionResult GetBookByPublicationYear(int year)
        {
            var pubyear = _context.Books.Where(p => p.PublicationYear == year).ToList();

            if (pubyear.Any())
            {
                return Ok(pubyear);

            }
            else
            {
                return NotFound("Books Not Found For Publication Year");
            }
        }
        [HttpGet]
        [Route("GetBookByGeneration")]

        public IActionResult GetBookByGeneration(int generation)
        {
            var GENERATION = _context.Books.Where(p => p.Generation == generation).ToList();

            if (GENERATION.Any())
            {
                return Ok(GENERATION);

            }
            else
            {
                return NotFound("Books Not Found For Requested Generation");
            }
        }

        [HttpGet]
        [Route("GetAvailableBook")]
        public IActionResult GetAvailableBook()
        {
            var availableBook = _context.Books.Where(p => p.Status == "Available").ToList();

            if (availableBook.Any())
            {
                return Ok(availableBook);

            }
            else
            {
                return NotFound("Books Not Available For Borrowing");
            }
        }


        [HttpGet]
        [Route("GetCheckedOutBook")]
        public IActionResult GetCheckedOutBook()
        {
            var CheckedOutBook = _context.Books.Where(p => p.Status == "Checked Out").ToList();

            if (CheckedOutBook.Any())
            {
                return Ok(CheckedOutBook);

            }
            else
            {
                return NotFound("Books are Available For Borrowing");
            }
        }
        [HttpGet]
        [Route("GetReservedBook")]
        public IActionResult GetReservedBook()
        {
            var ReservedBook = _context.Books.Where(p => p.Status == "Reserved").ToList();

            if (ReservedBook.Any())
            {
                return Ok(ReservedBook);

            }
            else
            {
                return NotFound("Requested books are  not available");
            }
        }




    }
}

