using BookReservationModel.Model;
using BookReservationDL.DatabaseContext;
using Microsoft.Extensions.Logging;
using BookReservationModel.DisplayModel;

namespace BookReservationDL.DataAccessLayer
{
    public class ReservationDL : IReservationDL
    {
        private readonly ILogger<object> _logger;
        private readonly SystemDbContext _dbContext;

        public ReservationDL(ILogger<object> logger, SystemDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<List<Reservation>?> GetReservations()
        {
            return _dbContext.Reservations.ToList();
        }

        public async Task<Reservation?> GetReservation(int id)
        {
            return _dbContext.Reservations.Find(id);
        }

        public async Task CreateReservation(Reservation reservation)
        {
            _dbContext.Reservations.Add(reservation);
            _dbContext.SaveChanges();
        }

        public async Task UpdateReservation(Reservation reservation)
        {
            _dbContext.Reservations.Update(reservation);
            _dbContext.SaveChanges();
        }

        public async Task DeleteReservation(Reservation reservation)
        {
            _dbContext.Reservations.Remove(reservation);
            _dbContext.SaveChanges();
        }

        public async Task<List<Reservation>?> GetLatestReservations()
        {
            return _dbContext.Reservations
                .GroupBy(r => r.BookId)
                .Select(group => group.OrderByDescending(r => r.StatusChangingDate).FirstOrDefault())
                .ToList();
        }

        public async Task<Reservation?> GetLatestReservationByBookId(int bookId)
        {
            List<Reservation>? latestReservations = await GetLatestReservations();

            if (latestReservations == null)
            {
                return null;
            }

            return latestReservations
                .Where(r => r.BookId == bookId)
                .FirstOrDefault();
        }
    }
}
