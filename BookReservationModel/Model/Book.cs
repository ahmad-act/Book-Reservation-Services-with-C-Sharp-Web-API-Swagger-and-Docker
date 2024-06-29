using System.ComponentModel.DataAnnotations;

namespace BookReservationModel.Model
{
    /// <summary>
    /// Represents a book in the reservation system.
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Gets or sets the unique identifier of the book.
        /// </summary>
        [Required]
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the book.
        /// </summary>
        [Required]
        [MinLength(3)]
        [MaxLength(150)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the author of the book.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets additional comments or notes about the book.
        /// </summary>
        [MaxLength(500)]
        public string? Note { get; set; }
    }
}
