
namespace BookReservationModel.DisplayModel
{
    /// <summary>
    /// Represents a model for displaying reserved book information on the UI.
    /// </summary>
    public class ReservedBook
    {
        /// <summary>
        /// Display the unique identifier for the reserved book.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Display the title of the reserved book.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Display the author of the reserved book.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Display the additional comments or notes related to the reservation.
        /// </summary>
        public string? ReservationComment { get; set; }

    }
}
