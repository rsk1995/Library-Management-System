using Library_Management_System.Models;
using Library_Management_System.Repository;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IBookManagement _bookManagement;
        public BooksManagementController(LMSDbContext context, IBookManagement bookManagement)
        {
            _context = context;
            _bookManagement = bookManagement;
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("AddBooks")]

        public async Task<IActionResult> AddNewBook([FromBody] BookDTO book)
        {
            if (book == null)
            {
                return BadRequest();
            }

            var createdBook=await _bookManagement.AddBook(book);
            return Ok("Book added successfully");

        }

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetAllBooks")]
        public async Task<ActionResult<IEnumerable<Books>>> GetAllBooks()
        {
            var AllBooks=await _bookManagement.GetAllBooks();
            return Ok(AllBooks);
        }

        //[Authorize(Roles = "Admin,Librarian")]
        [HttpGet]
        [Route("GetBookById")]

        public async Task<ActionResult<Books>> GetBookById(int id)
        {
            var book = await _bookManagement.GetBookById(id);
            if (book == null)
            {
                return NotFound("book Not Found");
            }
            else
            {
                return Ok(book);
            }
        }

        //[Authorize(Roles = "Admin,Librarian")]
        [HttpGet]
        [Route("GetBookByTitle")]

        public async Task<ActionResult<Books>> GetBookByTitle(string title)
        {
            var Book  = await _bookManagement.GetBookByTitle(title);

            if (Book.Any())
            {
                return Ok(Book);

            }
            else
            {
                return NotFound("Books Not Found For Requested Title");
            }
        }

        //[Authorize(Roles = "Admin,Librarian")]
        [HttpGet]
        [Route("GetBookByAuthor")]

        public async Task<ActionResult<Books>> GetBookByAuthor(string author)
        {
            var Bauthor = await _bookManagement.GetBookByAuthor(author);

            if (Bauthor.Any())
            {
                return Ok(Bauthor);

            }
            else
            {
                return NotFound("Books Not Found For Requested Author");
            }
        }

        //[Authorize(Roles = "Admin,Librarian")]
        [HttpGet]
        [Route("GetBookByPublicationYear")]

        public async Task<ActionResult<Books>> GetBookByPublicationYear(int year)
        {
            var pubyear = await _bookManagement.GetBookByPublicationYear(year);

            if (pubyear.Any())
            {
                return Ok(pubyear);

            }
            else
            {
                return NotFound("Books Not Found For Publication Year");
            }
        }

        //[Authorize(Roles = "Admin,Librarian")]
        [HttpGet]
        [Route("GetBookByGeneration")]

        public async Task<ActionResult<Books>> GetBookByGeneration(int generation)
        {
            if (generation == 0)
            {
                return BadRequest("Enter book generation!"
);          }

            var gen = await _bookManagement.GetBookByPublicationYear(generation);

            if (gen.Any())
            {
                return Ok(gen);

            }
            else
            {
                return NotFound("Books Not Found For Publication Year");
            }

        }

        //[Authorize(Roles = "Admin,Librarian")]
        [HttpGet]
        [Route("GetAvailableBook")]
        public async Task<ActionResult<Books>> GetAvailableBooks()
        {
            var Book = await _bookManagement.GetAvailableBooks();

            if (Book.Any())
            {
                return Ok(Book);
            }
            else
            {
                return NotFound("Available books not found");
            }
        }

        //[Authorize(Roles = "Admin,Librarian")]
        [HttpGet]
        [Route("GetCheckedOutBook")]
        public async Task<ActionResult<Books>> GetCheckedOutBook()
        {
            var Book = await _bookManagement.GetCheckedOutBooks();

            if (Book.Any())
            {
                return Ok(Book);
            }
            else
            {
                return NotFound("Books are Available For Borrowing");
            }
        }

        //[Authorize(Roles = "Admin,Librarian")]
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