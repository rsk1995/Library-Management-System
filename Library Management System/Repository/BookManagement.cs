using System.Runtime.InteropServices;
using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Repository
{
    public class BookManagement : IBookManagement
    {
        private readonly LMSDbContext _context;
        public BookManagement(LMSDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Books>> GetAllBooks()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Books> GetBookById(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<BookDTO> AddBook(BookDTO book)
        {
            await _context.Books.AddAsync(new Books
            {
                Title = book.Title,
                Author = book.Author,
                Generation = book.Generation,
                ISBN = book.ISBN,
                PublicationYear = book.PublicationYear,
                Status = book.Status
            });
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<IEnumerable<Books>> GetBookByTitle(string title)
        {
            return await _context.Books.Where(p => p.Title.Contains(title)).ToListAsync();
        }

        public async Task<IEnumerable<Books>> GetBookByAuthor(string author)
        {
            return await _context.Books.Where(p => p.Author.Contains(author)).ToListAsync();
        }

        public async Task<IEnumerable<Books>> GetBookByPublicationYear(int year)
        {
            return await _context.Books.Where(p => p.PublicationYear.Equals(year)).ToListAsync();
        }

        public async Task<IEnumerable<Books>> GetBookByGenration(int gen)
        {
            return await _context.Books.Where(p => p.Generation.Equals(gen)).ToListAsync();
        }

        public async Task<IEnumerable<Books>> GetAvailableBooks()
        {
            return await _context.Books.Where(p => p.Status.Contains("Available")).ToListAsync();
        }

        public async Task<IEnumerable<Books>> GetCheckedOutBooks()
        {
            return await _context.Books.Where(p => p.Status.Contains("Checked Out")).ToListAsync();
        }

        public async Task<IEnumerable<Books>> GetReservedBook()
        {
            return await _context.Books.Where(p => p.Status.Contains("Reserved Book")).ToListAsync();
        }    
    }
}
