using BookReservationModel.Model;
using BookReservationDL.DatabaseContext;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using BookReservationModel.DisplayModel;

namespace BookReservationDL.DataAccessLayer
{
    public class BookDL : IBookDL
    {
        private readonly ILogger<object> _logger;
        private readonly SystemDbContext _dbContext;

        public BookDL(ILogger<object> logger, SystemDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<List<Book>?> GetBooks()
        {
            return _dbContext.Books.ToList();
        }

        public async Task<Book?> GetBook(int id)
        {
            return _dbContext.Books.Find(id);
        }

        public async Task CreateBook(Book book)
        {
            _dbContext.Books.Add(book);
            _dbContext.SaveChanges();
        }

        public async Task UpdateBook(Book book)
        {
            _dbContext.Books.Update(book);
            _dbContext.SaveChanges();
        }

        public async Task DeleteBook(Book book)
        {
            _dbContext.Books.Remove(book);
            _dbContext.SaveChanges();
        }

        public async Task<List<Book>?> SearchBooks(string searchTerm)
        {
            return _dbContext.Books
                .Where(b => b.Title.ToLower().Contains(searchTerm.ToLower()) 
                || b.Author.ToLower().Contains(searchTerm.ToLower()))
                .ToList();
        }
    }
}
