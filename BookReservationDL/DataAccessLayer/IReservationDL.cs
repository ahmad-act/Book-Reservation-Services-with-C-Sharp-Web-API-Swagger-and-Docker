using BookReservationModel.DisplayModel;
using BookReservationModel.Model;

namespace BookReservationDL.DataAccessLayer
{
    public interface IReservationDL
    {
        Task<List<Reservation>?> GetReservations();
        Task<Reservation?> GetReservation(int id);
        Task CreateReservation(Reservation reservation);
        Task UpdateReservation(Reservation reservation);
        Task DeleteReservation(Reservation reservation);
        Task<List<Reservation>?> GetLatestReservations();
        Task<Reservation?> GetLatestReservationByBookId(int bookId);
    }
}