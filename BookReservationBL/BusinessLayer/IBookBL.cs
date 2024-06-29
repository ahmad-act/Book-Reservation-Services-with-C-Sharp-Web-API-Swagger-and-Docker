using BookReservationModel.DisplayModel;
using BookReservationModel.Model;

namespace BookReservationBL.BusinessLayer
{
    public interface IBookBL
    {
        Task<List<Book>?> GetBooks();
        Task<Book?> GetBook(int id);
        Task CreateBook(Book book);
        Task<Book?> UpdateBook(int id, Book book);
        Task<string> DeleteBook(int id);
        Task<List<Book>?> SearchBooks(string searchTerm);
    }
}