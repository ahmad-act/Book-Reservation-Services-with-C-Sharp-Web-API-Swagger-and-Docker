using BookReservationModel.Model;
using BookReservationDL.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BookReservationModel.DisplayModel;

namespace BookReservationBL.BusinessLayer
{
    public class ReservationBL : IReservationBL
    {
        private readonly ILogger<object> _logger;
        private readonly IReservationDL _reservationDL;
        private readonly IBookBL _bookBL;

        public ReservationBL(ILogger<object> logger, IReservationDL reservationDL, IBookBL bookBL)
        {
            _logger = logger;
            _reservationDL = reservationDL;
            _bookBL = bookBL;
        }

        public async Task<List<Reservation>?> GetReservations()
        {
            return await _reservationDL.GetReservations();
        }

        public async Task<Reservation?> GetReservation(int id)
        {
            return await _reservationDL.GetReservation(id);
        }

        public async Task<string> CreateReservation(Reservation reservation)
        {
            if (reservation == null)
            {
                return "Invalid reservation";
            }

            await _reservationDL.CreateReservation(reservation);

            return string.Empty;
        }

        public async Task<string> UpdateReservation(int id, Reservation reservation)
        {
            var existingReservation = await GetReservation(id);

            if (existingReservation == null)
            {
                return "The reservation is not found.";
            }

            existingReservation.BookId = reservation.BookId;
            existingReservation.StatusChangingDate = reservation.StatusChangingDate;
            existingReservation.Status = reservation.Status;
            existingReservation.Comment = reservation.Comment;

            await _reservationDL.UpdateReservation(existingReservation);

            return string.Empty;
        }

        public async Task<string> DeleteReservation(int id)
        {
            var reservation = await GetReservation(id);

            if (reservation == null)
            {
                return "Invalid reservation";
            }

            await _reservationDL.DeleteReservation(reservation);

            return string.Empty;
        }

        public async Task<List<Reservation>?> LatestReservations()
        {
            return await _reservationDL.GetLatestReservations();
        }

        public async Task<List<ReservedBook>?> GetReservedBooks()
        {
            List<Reservation>? latestReservations = await _reservationDL.GetLatestReservations();

            if (latestReservations == null)
            {
                return null;
            }

            List<int> reservedBookIds = latestReservations
                .Where(r => r.Status == ReservationStatus.CheckedOut)
                .Select(r => r.BookId)
                .ToList();

            List<Book>? books = await _bookBL.GetBooks();

            if (books == null)
            {
                return null;
            }

            List<Book> reservedBooks = books
                .Where(b => reservedBookIds.Contains(b.Id))
                .ToList();

            List<ReservedBook> result = (from b in reservedBooks
                                         join r in latestReservations on b.Id equals r.BookId
                                         select new ReservedBook
                                         {
                                             Id = b.Id,
                                             Title = b.Title,
                                             Author = b.Author,
                                             ReservationComment = r.Comment
                                         }).ToList();

            return result;
        }

        public async Task<List<Book>?> GetAvailableBooks()
        {
            List<Reservation>? latestReservations = await _reservationDL.GetLatestReservations();

            if (latestReservations == null)
            {
                return null;
            }

            List<int> reservedBookIds = latestReservations
                .Where(r => r.Status == ReservationStatus.CheckedOut)
                .Select(r => r.BookId)
                .ToList();

            List<Book> books = await _bookBL.GetBooks();

            if (books == null)
            {
                return null;
            }

            List<Book> availableBooks = books
                .Where(b => !reservedBookIds.Contains(b.Id))
                .ToList();

            return availableBooks;
        }

        public async Task<string> ReserveBook(int bookId, string? comment)
        {
            Book? book = await _bookBL.GetBook(bookId);

            if (book == null)
            {
                return "Please enter a valid Book ID";
            }

            Reservation? existingReservation = await _reservationDL.GetLatestReservationByBookId(bookId);

            if (existingReservation != null 
                && existingReservation.Status == ReservationStatus.CheckedOut)
            {
                return "The book is already reserved";
            }

            Reservation reservation = new Reservation()
            {
                BookId = bookId,
                StatusChangingDate = DateTime.UtcNow,
                Status = ReservationStatus.CheckedOut,
                Comment = comment
            };

            return await CreateReservation(reservation);
        }

        public async Task<string> ReturnReservedBook(int bookId, string? comment)
        {
            Book? book = await _bookBL.GetBook(bookId);

            if (book == null)
            {
                return "Please enter a valid Book ID";
            }

            Reservation? existingReservation = await _reservationDL.GetLatestReservationByBookId(bookId);

            if (existingReservation != null
                && existingReservation.Status == ReservationStatus.CheckedOut)
            {
                Reservation reservation = new Reservation()
                {
                    BookId = bookId,
                    StatusChangingDate = DateTime.UtcNow,
                    Status = ReservationStatus.Returned,
                    Comment = comment
                };

                return await CreateReservation(reservation);
            }

            return "The book is not reserved";
        }

        public async Task<List<ReservationHistory>?> GetReservationHistory()
        {
            List<Book>? books = await _bookBL.GetBooks();

            if (books == null)
            {
                return null;
            }

            List<Reservation>? reservations = await GetReservations();

            if (reservations == null)
            {
                return null;
            }

            var joinResults = books
                .Join(
                    reservations,
                    book => book.Id,
                    reservation => reservation.BookId,
                    (book, reservation) => new
                    {
                        book.Id,
                        book.Title,
                        book.Author,
                        book.Note,
                        Reservation = reservation
                    })
                .ToList();

            var groupedResults = joinResults
                .GroupBy(result => new
                {
                    result.Id,
                    result.Title,
                    result.Author,
                    result.Note
                })
                .Select(group => new ReservationHistory
                {
                    Id = group.Key.Id,
                    Title = group.Key.Title,
                    Author = group.Key.Author,
                    Comment = group.Key.Note,
                    Reservations = group.Select(result => result.Reservation).ToList()
                })
                .ToList();

            return groupedResults;
        }
    }
}
