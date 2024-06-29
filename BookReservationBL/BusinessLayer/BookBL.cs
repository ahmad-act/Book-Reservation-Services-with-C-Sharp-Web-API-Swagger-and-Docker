using BookReservationModel.Model;
using BookReservationDL.DataAccessLayer;
using Microsoft.Extensions.Logging;

namespace BookReservationBL.BusinessLayer
{
    public class BookBL : IBookBL
    {
        private readonly ILogger<object> _logger;
        private readonly IBookDL _bookDL;

        public BookBL(ILogger<object> logger, IBookDL bookDL)
        {
            _logger = logger;
            _bookDL = bookDL;
        }

        public async Task<List<Book>?> GetBooks()
        {
            return await _bookDL.GetBooks();
        }

        public async Task<Book?> GetBook(int id)
        {
            return await _bookDL.GetBook(id);
        }

        public async Task CreateBook(Book book)
        {
            await _bookDL.CreateBook(book);
        }

        public async Task<Book?> UpdateBook(int id, Book book)
        {
            if (book == null)
            {
                return null;
            }

            var existingBook = await GetBook(id);

            if (existingBook == null)
            {
                return null;
            }

            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.Note = book.Note;

            await _bookDL.UpdateBook(existingBook);

            return existingBook;
        }

        public async Task<string> DeleteBook(int id)
        {
            var book = await GetBook(id);

            if (book == null)
            {
                return "Book not found";
            }

            await _bookDL.DeleteBook(book);

            return string.Empty;
        }

        public async Task<List<Book>?> SearchBooks(string searchTerm)
        {
            return await _bookDL.SearchBooks(searchTerm);
        }
    }
}
