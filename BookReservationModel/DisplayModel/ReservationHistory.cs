using BookReservationModel.Model;

namespace BookReservationModel.DisplayModel
{
    /// <summary>
    /// Represents a model for displaying reservation history for a book.
    /// </summary>
    public class ReservationHistory
    {
        /// <summary>
        /// Display the unique identifier for the book.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Display the title of the book.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Display the author of the book.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Display additional comments or notes related to the book.
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// Display the list of reservations associated with the book.
        /// </summary>
        public List<Reservation> Reservations { get; set; }
    }
}
