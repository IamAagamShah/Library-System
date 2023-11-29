using BookHubAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace BookHubAPI.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BooksDbContext _context;

        public BookRepository(BooksDbContext context)
        {
            _context = context;
        }
        public async Task<Book> PartialUpdateBookAsync(string id, Book book)
        {
            var existingBook = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (existingBook == null) return null;

            // Apply partial updates here as needed
            if (!string.IsNullOrEmpty(book.Title))
                existingBook.Title = book.Title;

            // Update other properties similarly...

            await _context.SaveChangesAsync();
            return existingBook;
        }


        public async Task<IEnumerable<Book>> GetAllBooks()
        {           return await _context.Books.ToListAsync();
        }

        public async Task<Book> GetBookById(string id)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book> AddBook(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

   
        public async Task<Book> UpdateBook(string id, Book book)
        {
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<bool> DeleteBook(string id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null) return false;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
