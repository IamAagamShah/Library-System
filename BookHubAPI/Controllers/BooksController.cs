
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        public BookController(IBookRepository bookRepository, IReviewRepository reviewRepository, IMapper mapper)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("getitems")]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
        {
            var books = await _bookRepository.GetAllBooks();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBookById(string id, bool includeReviews = false)
        {
            var book = await _bookRepository.GetBookById(id, includeReviews);
            if (book == null)
            {
                return NotFound();
            }
            if (includeReviews)
            {
                var reviews = _mapper.Map<BookDTO>(book);
                return Ok(book);
            }
            return Ok(book);
        }

        [HttpPost]
        [Route("addbook")]
        public async Task<ActionResult<BookDTO>> AddBook(BookDTO bookDto)
        {
            var books = _mapper.Map<Book>(bookDto);
            var addedBook = await _bookRepository.AddBook(books);
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
            var updatedBookDto = _mapper.Map<BookDTO>(updatedBook);
            return Ok(updatedBookDto);
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