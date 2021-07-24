using AppointmentBooking.Domain;
using AppointmentBooking.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AppointmentBookingAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppointmentBookingController : ControllerBase
    {
        private readonly ILogger<AppointmentBookingController> _logger;
        private readonly IBookingService _bookingService;

        public AppointmentBookingController(ILogger<AppointmentBookingController> logger, IBookingService bookingService)
        {
            _logger = logger;
            _bookingService = bookingService;
        }

        /// <summary>
        /// Action to book a new appointment.
        /// </summary>
        /// <param name="request">Model to book a new appointmnet.</param>
        /// <returns>Returns the booked appointmnet id.</returns>
        /// <response code="200">Returned if the appointmnet is successfully booked.</response>
        /// <response code="400">Returned if the data passed is not correct, or appointment is requested outside the business hours.</response>
        /// <response code="409">Returned when there is no appointment available at the requested time.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPost]
        public async Task<IActionResult> BookAppointmentAsync([FromBody] AppointmentBookingRequest request)
        {
            _logger.LogInformation("Appointment request received by controller.");

            // call service
            var response = await _bookingService.BookAppointmentAsync(request);

            _logger.LogInformation("Response received from service in controller.");

            // build response, based on reponse received from service
            return response.BookingId.Equals(Guid.Empty) ? Conflict("There is no appointment available at the selected time.") : Ok(response);
        }       
    }
}
