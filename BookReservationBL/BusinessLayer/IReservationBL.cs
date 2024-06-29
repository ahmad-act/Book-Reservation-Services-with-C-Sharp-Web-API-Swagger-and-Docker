using BookReservationModel.DisplayModel;
using BookReservationModel.Model;

namespace BookReservationBL.BusinessLayer
{
    public interface IReservationBL
    {
        Task<List<Reservation>?> GetReservations();
        Task<Reservation?> GetReservation(int id);
        Task<string> CreateReservation(Reservation reservation);
        Task<string> UpdateReservation(int id, Reservation reservation);
        Task<string> DeleteReservation(int id);
        Task<List<Reservation>?> LatestReservations();
        Task<List<ReservedBook>?> GetReservedBooks();
        Task<List<Book>?> GetAvailableBooks();
        Task<string> ReserveBook(int bookId, string? comment);
        Task<string> ReturnReservedBook(int bookId, string? comment);
        Task<List<ReservationHistory>?> GetReservationHistory();
    }
}