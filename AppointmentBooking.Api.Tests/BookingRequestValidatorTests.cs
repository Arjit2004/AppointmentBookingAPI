using AppointmentBooking.API.Validators;
using AppointmentBooking.Domain;
using FluentValidation.TestHelper;
using Xunit;

namespace AppointmentBooking.Api.Tests
{
    public class BookingRequestValidatorTests
    {
        private readonly BookingRequestValidator _validator;

        public BookingRequestValidatorTests()
        {
            _validator = new BookingRequestValidator();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Name_WhenNullOrEmpty_ShouldHaveValidationError(string name)
        {
            var request = new AppointmentBookingRequest()
            {
                Name = name,
                BookingTime = "09:00"
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(request => request.Name);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void BookingTime_WhenNullOrEmpty_ShouldHaveValidationError(string bookingTime)
        {
            var request = new AppointmentBookingRequest()
            {
                Name = "name",
                BookingTime = bookingTime
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(request => request.BookingTime);
        }

        [Theory]
        [InlineData("test")]
        [InlineData("08:59")]
        [InlineData("16:01")]
        [InlineData("12:00:00")]
        public void BookingTime_WhenInvalidOrOutsideWorkingHours_ShouldHaveValidationError(string bookingTime)
        {
            var request = new AppointmentBookingRequest()
            {
                Name = "name",
                BookingTime = bookingTime
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(request => request.BookingTime);
        }
    }
}
