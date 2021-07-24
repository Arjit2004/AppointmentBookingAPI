using AppointmentBooking.Domain;
using FluentValidation;
using System;
using System.Globalization;

namespace AppointmentBooking.API.Validators
{
    public class BookingRequestValidator : AbstractValidator<AppointmentBookingRequest>
    {
        public BookingRequestValidator()
        {
            RuleFor(request => request.Name).NotEmpty().WithMessage("Name must be provided for booking a settlement.");
            RuleFor(request => request.BookingTime).NotEmpty().WithMessage("Booking time needs to be mentioned for booking a settlement.");

            RuleFor(request => request.BookingTime).ValidateBookingTime();
            RuleFor(request => request.BookingTime).ValidateTimeAgainstOfficeHours();
        }
    }

    public static class BookingTimeValidator
    {
        public static IRuleBuilderOptions<T, string> ValidateBookingTime<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            DateTime bookingTime;

            return ruleBuilder.Must(time => DateTime.TryParseExact(time, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out bookingTime) == true)
                .WithMessage("Booking time is not in valid format. It should be a 24-hour time (09:00 - 05:00)");
        }

        public static IRuleBuilderOptions<T, string> ValidateTimeAgainstOfficeHours<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var startTime = new TimeSpan(8, 59, 0);
            var endTime = new TimeSpan(16, 0, 0);
            DateTime bookingTime;

            return ruleBuilder.Must(time => DateTime.TryParseExact(time, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out bookingTime) == true
                && DateTime.ParseExact(time, "HH:mm", CultureInfo.InvariantCulture).TimeOfDay > startTime
                && DateTime.ParseExact(time, "HH:mm", CultureInfo.InvariantCulture).TimeOfDay <= endTime)
                .WithMessage("Booking time requested lies outside the working hours.");
        }
    }
}
