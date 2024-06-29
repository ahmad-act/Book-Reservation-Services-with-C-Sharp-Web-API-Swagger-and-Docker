using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookReservationModel.Model
{
    /// <summary>
    /// Represents a reservation entity.
    /// </summary>
    public class Reservation
    {
        /// <summary>
        /// Gets or sets the unique identifier for the reservation.
        /// </summary>
        [Required]
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the foreign key referencing the associated book.
        /// </summary>
        [Required]
        [ForeignKey("Book")]
        public int BookId { get; set; }

        /// <summary>
        /// Gets or sets the date when the reservation status changed.
        /// </summary>
        [Required]
        public DateTime StatusChangingDate { get; set; }

        /// <summary>
        /// The status of the reservation.
        /// </summary>
        /// <example>0 = CheckedOut</example>
        /// <example>1 = Returned</example>
        [Required]
        public ReservationStatus Status { get; set; }

        /// <summary>
        /// Gets or sets additional comments or notes about the reservation.
        /// </summary>
        [MaxLength(500)]
        public string? Comment { get; set; }
    }

    /// <summary>
    /// Enumeration representing reservation statuses.
    /// 0 = CheckedOut
    /// 1 = Returned
    /// </summary>
    public enum ReservationStatus
    {
        /// <summary>
        /// The book is currently checked out.
        /// </summary>
        CheckedOut,

        /// <summary>
        /// The book has been returned.
        /// </summary>
        Returned
    }
}
