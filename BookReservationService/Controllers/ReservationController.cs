using BookReservationBL.BusinessLayer;
using BookReservationModel.DisplayModel;
using BookReservationModel.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace BookReservationService.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    [Produces("application/json")]
    public class ReservationController : Controller
    {
        private readonly ILogger<object> _logger;
        private readonly IReservationBL _reservationBL;

        public ReservationController(ILogger<object> logger, IReservationBL reservationBL)
        {
            _logger = logger;
            _reservationBL = reservationBL;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all reservations")]
        [SwaggerResponse(200, "Successfully retrieved reservations", typeof(List<Reservation>))]
        [SwaggerResponse(404, "Reservation not found")]
        [SwaggerResponse(500, "Internal Server Error")]
        private async Task<IActionResult> GetReservations()
        {
            try
            {
                var reservasions = await _reservationBL.GetReservations();

                if (reservasions == null || reservasions.Count == 0)
                {
                    return NotFound("Reservations not found");
                }

                return Ok(reservasions);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to complete GetReservations(). \nHTTP status code: 500 \nError: {Message}", ex.Message);
                return StatusCode(500, "Internal server error occurred");
            }

        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get a reservation by ID")]
        [SwaggerResponse(200, "Successfully retrieved the book", typeof(Reservation))]
        [SwaggerResponse(404, "Reservation not found")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "Internal Server Error")]
        private async Task<IActionResult> GetReservation(int id)
        {
            try
            {
                var reservation = await _reservationBL.GetReservation(id);

                if (reservation == null)
                {
                    return NotFound("Reservation not found");
                }

                return Ok(reservation);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to complete GetReservation(). \nHTTP status code: 500 \nError: {Message}", ex.Message);
                return StatusCode(500, "Internal server error occurred");
            }

        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove a reservation")]
        [SwaggerResponse(204, "Reservation removed successfully", typeof(Reservation))]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "Internal Server Error")]
        private async Task<IActionResult> DeleteReservation(int id)
        {
            try
            {
                var msg = await _reservationBL.DeleteReservation(id);

                if (!string.IsNullOrEmpty(msg))
                {
                    return BadRequest(msg);
                }

                return Ok("Successfully deleted the reservation");
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to complete DeleteReservation(). \nHTTP status code: 500 \nError: {Message}", ex.Message);
                return StatusCode(500, "Internal server error occurred");
            }
        }

        [HttpGet("reserved_book")]
        [SwaggerOperation(Summary = "Get list of reserved books with reservation comment")]
        [SwaggerResponse(200, "Successfully retrieved reserved books", typeof(List<ReservedBook>))]
        [SwaggerResponse(404, "No reserved books found")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> GetReservedBooks()
        {
            try
            {
                var reservedBooks = await _reservationBL.GetReservedBooks();

                if (reservedBooks == null || reservedBooks.Count == 0)
                {
                    return NotFound("No reserved books found");
                }

                return Ok(reservedBooks);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to complete GetReservedBooks(). \nHTTP status code: 500 \nError: {Message}", ex.Message);
                return StatusCode(500, "Internal server error occurred");
            }
        }

        [HttpGet("available_book")]
        [SwaggerOperation(Summary = "Get list of available (not reserved) books")]
        [SwaggerResponse(200, "Successfully retrieved available books", typeof(List<Book>))]
        [SwaggerResponse(404, "No available books found")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> GetAvailableBooks()
        {
            try
            {
                var availableBooks = await _reservationBL.GetAvailableBooks();

                if (availableBooks == null || availableBooks.Count == 0)
                {
                    return NotFound("No available books found");
                }

                return Ok(availableBooks);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to complete GetAvailableBooks(). \nHTTP status code: 500 \nError: {Message}", ex.Message);
                return StatusCode(500, "Internal server error occurred");
            }
        }

        [HttpPost("reserve/{bookId}")]
        [SwaggerOperation(Summary = "Reserve a book by the book ID")]
        [SwaggerResponse(204, "Reservation completed successfully", typeof(Reservation))]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> ReserveBook(int bookId, string? comment)
        {
            try
            {
                var msg = await _reservationBL.ReserveBook(bookId, comment);

                if (!string.IsNullOrEmpty(msg))
                {
                    return BadRequest(msg);
                }

                return Ok("Reservation completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to complete ReserveBook(). \nHTTP status code: 500 \nError: {Message}", ex.Message);
                return StatusCode(500, "Internal server error occurred");
            }

        }

        [HttpPost("return/{bookId}")]
        [SwaggerOperation(Summary = "Return a book by the book ID")]
        [SwaggerResponse(204, "Reservation return completed successfully", typeof(Reservation))]
        [SwaggerResponse(404, "Reservation not found")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> ReturnReservedBook(int bookId, string? comment)
        {
            try
            {
                var msg = await _reservationBL.ReturnReservedBook(bookId, comment);

                if (!string.IsNullOrEmpty(msg))
                {
                    return BadRequest(msg);
                }

                return Ok("Reservation return completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to complete UpdateReservationStatus(). \nHTTP status code: 500 \nError: {Message}", ex.Message);
                return StatusCode(500, "Internal server error occurred");
            }

        }

        [HttpGet("History")]
        [SwaggerOperation(Summary = "Get all reservation history")]
        [SwaggerResponse(200, "Successfully retrieved reservation history", typeof(List<ReservationHistory>))]
        [SwaggerResponse(404, "Reservation history not found")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> GetReservationHistory()
        {
            try
            {
                var reservasions = await _reservationBL.GetReservationHistory();

                if (reservasions == null || reservasions.Count == 0)
                {
                    return NotFound("Reservations not found");
                }

                return Ok(reservasions);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to complete GetReservations(). \nHTTP status code: 500 \nError: {Message}", ex.Message);
                return StatusCode(500, "Internal server error occurred");
            }

        }

    }
}
