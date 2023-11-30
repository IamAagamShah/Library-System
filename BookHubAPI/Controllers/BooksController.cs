
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure;
using BookHubAPI.Models;
using BookHubAPI.Repository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol.Core.Types;

namespace BookHubAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        }

        [HttpGet]
        [Route("getitems")]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
        {
            var books = await _bookRepository.GetAllBooks();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBookById(string id)
        {
            var book = await _bookRepository.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost]
        [Route("addbook")]
        public async Task<ActionResult<Book>> AddBook(Book book)
        {
            var addedBook = await _bookRepository.AddBook(book);
            return CreatedAtAction(nameof(GetBookById), new { id = addedBook.Id }, addedBook);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Book>> UpdateBook(string id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }
            var updatedBook = await _bookRepository.UpdateBook(id, book);
            if (updatedBook == null)
            {
                return NotFound();
            }
            return Ok(updatedBook);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(string id)
        {
            var result = await _bookRepository.DeleteBook(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}