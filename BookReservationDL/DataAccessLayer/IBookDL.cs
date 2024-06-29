using BookReservationModel.DisplayModel;
using BookReservationModel.Model;

namespace BookReservationDL.DataAccessLayer
{
    public interface IBookDL
    {
        Task<List<Book>?> GetBooks();
        Task<Book?> GetBook(int id);
        Task CreateBook(Book book);
        Task UpdateBook(Book book);
        Task DeleteBook(Book book);
        Task<List<Book>?> SearchBooks(string searchTerm);
    }
}