using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Repository
{
    public interface IBookManagement
    {
        Task<BookDTO> AddBook(BookDTO book);
        Task<IEnumerable<Books>> GetAllBooks();
        Task<Books> GetBookById(int id);
        Task<IEnumerable<Books>> GetBookByTitle(string title);

        Task<IEnumerable<Books>> GetBookByAuthor(string author);

        Task<IEnumerable<Books>> GetBookByPublicationYear(int year);

        Task<IEnumerable<Books>> GetBookByGenration(int gen);
        Task<IEnumerable<Books>> GetAvailableBooks();
        Task<IEnumerable<Books>> GetCheckedOutBooks();
        Task<IEnumerable<Books>> GetReservedBook();



    }
}
