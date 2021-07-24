using System;

namespace AppointmentBooking.Domain.Entities
{
    public partial class Booking
    {
        public Guid BookingId { get; set; }

        public TimeSpan BookingTime { get; set; }

        public string Name { get; set; }
    }
}
