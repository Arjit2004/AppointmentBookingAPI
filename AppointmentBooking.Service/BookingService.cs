using AppointmentBooking.Data.Database;
using AppointmentBooking.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentBooking.Service
{
    public interface IBookingService
    {
        public Task<AppointmentBookingResponse> BookAppointmentAsync(AppointmentBookingRequest request);
    }

    /// <summary>
    /// To facilitate the booking of an appointmnet, based on the avilability.
    /// </summary>
    public class BookingService : IBookingService
    {
        private readonly ILogger<BookingService> _logger;
        private readonly BookingContext _dbContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dbContext"></param>
        public BookingService(ILogger<BookingService> logger, BookingContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<AppointmentBookingResponse> BookAppointmentAsync(AppointmentBookingRequest request)
        {
            _logger.LogInformation("Appointment request received by the service.");

            try
            {
                var time = DateTime.ParseExact(request.BookingTime, "HH:mm", CultureInfo.InvariantCulture).TimeOfDay;

                if (CheckAppointmentAvailability(time))
                {
                    var booking = new Domain.Entities.Booking()
                    {
                        BookingTime = time,
                        Name = request.Name
                    };

                    _dbContext.Booking.Add(booking);

                    await _dbContext.SaveChangesAsync();

                    _logger.LogInformation("Appointment booked successfully.");

                    return new AppointmentBookingResponse()
                    {
                        BookingId = booking.BookingId
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception {0} has been caught in the booking service. Exception details: {1}", ex.GetType(), ex.Message);
            }


            _logger.LogInformation("Appointment couldn't be booked.");
            return new AppointmentBookingResponse()
            {
                BookingId = Guid.Empty
            };
        }

        /// <summary>
        /// To check the appointment availability at the requested time.
        /// </summary>
        /// <param name="time">Time, at which the appoitmnet is requested.</param>
        /// <returns>True, if appointment is available, false otherwise.</returns>
        private bool CheckAppointmentAvailability(TimeSpan time)
        {
            var existingBookings =  _dbContext.Booking
                .Where(b => (b.BookingTime <= time && b.BookingTime > time.Subtract(new TimeSpan(1,0,0)))
                || (b.BookingTime > time && b.BookingTime < time.Add(new TimeSpan(1, 0, 0))))
                .Count();

            if (existingBookings < 4)
            {
                _logger.LogInformation("Appoitment available at requested time.");
                return true;
            }
            else
            {
                _logger.LogInformation("Appoitment unavailable at requested time.");
                return false;
            }
        }
    }
}
