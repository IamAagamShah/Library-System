using BookHubAPI.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace BookHubAPI.Repository
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooks();
        Task<Book> GetBookById(string id);
        Task<Book> AddBook(Book book);
        Task<Book> UpdateBook(string id, Book book);
        Task<bool> DeleteBook(string id);
    }
}