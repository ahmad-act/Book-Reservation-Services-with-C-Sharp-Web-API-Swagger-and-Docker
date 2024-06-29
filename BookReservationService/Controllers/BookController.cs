using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using BookReservationBL.BusinessLayer;
using BookReservationModel.Model;

namespace BookReservationService.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    [Produces("application/json")]
    public class BookController : Controller
    {
        private readonly ILogger<object> _logger;
        private readonly IBookBL _bookBL;

        public BookController(ILogger<object> logger, IBookBL bookBL)
        {
            _logger = logger;
            _bookBL = bookBL;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all books")]
        [SwaggerResponse(200, "Successfully retrieved books", typeof(List<Book>))]
        [SwaggerResponse(404, "Books not found")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> GetBooks()
        {
            try
            {
                List<Book> books = await _bookBL.GetBooks();

                if (books == null || books.Count == 0)
                {
                    return NotFound("Books not found");
                }

                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to complete GetBooks(). \nHTTP status code: 500 \nError: {Message}", ex.Message);
                return StatusCode(500, "Internal server error occurred");
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get a book by ID")]
        [SwaggerResponse(200, "Successfully retrieved the book", typeof(Book))]
        [SwaggerResponse(404, "Book not found")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> GetBook(int id)
        {
            try
            {

                var book = await _bookBL.GetBook(id);

                if (book == null)
                {
                    return NotFound("Book not found");
                }

                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to complete GetBook(). \nHTTP status code: 500 \nError: {Message}", ex.Message);
                return StatusCode(500, "Internal server error occurred");
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new book")]
        [SwaggerResponse(201, "Successfully created the book", typeof(Book))]
        [SwaggerResponse(400, "Invalid book data")]
        [SwaggerResponse(500, "Internal server error occurred")]
        public async Task<IActionResult> CreateBook([FromBody] Book book)
        {
            try
            {

                if (book == null)
                {
                    return BadRequest("Invalid book data");
                }

                await _bookBL.CreateBook(book);

                return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to complete CreateBook(). \nHTTP status code: 500 \nError: {Message}", ex.Message);
                return StatusCode(500, "Internal server error occurred");
            }
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update a book by ID")]
        [SwaggerResponse(200, "Successfully updated the book")]
        [SwaggerResponse(404, "Book not found")]
        [SwaggerResponse(400, "Invalid book data")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Book book)
        {
            try
            {

                if (book == null)
                {
                    return BadRequest("Invalid book data");
                }

                var existingBook = await _bookBL.UpdateBook(id, book);

                if (existingBook == null)
                {
                    return NotFound("Book not found");
                }

                return Ok(existingBook);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to complete GetBooks(). \nHTTP status code: 500 \nError: {Message}", ex.Message);
                return StatusCode(500, "Internal server error occurred");
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a book by ID")]
        [SwaggerResponse(204, "Successfully deleted the book", typeof(Book))]
        [SwaggerResponse(404, "Book not found")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                string msg = await _bookBL.DeleteBook(id);

                if (!string.IsNullOrEmpty(msg))
                {
                    return NotFound(msg);
                }

                return Ok("Successfully deleted the book");
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to complete UpdateBook(). \nHTTP status code: 500 \nError: {Message}", ex.Message);
                return StatusCode(500, "Internal server error occurred");
            }
        }

        [HttpGet("search")]
        [SwaggerOperation(Summary = "Search book by Title or Author")]
        [SwaggerResponse(200, "Successfully searched the books", typeof(List<Book>))]
        [SwaggerResponse(204, "The search did not yield any results")]
        [SwaggerResponse(400, "Please enter search data")]
        [SwaggerResponse(404, "No books found matching the search term")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> SearchBooks(string? searchTerm)
        {
            try
            {

                if (string.IsNullOrEmpty(searchTerm))
                {
                    return BadRequest("Please enter search data");
                }

                var books = await _bookBL.SearchBooks(searchTerm);

                if (books == null || books.Count == 0)
                {
                  return  NotFound("No books found matching the search term");
                }

                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to complete SearchBooks(). \nHTTP status code: 500 \nError: {Message}", ex.Message);
                return StatusCode(500, "Internal server error occurred");
            }
        }
    }
}
