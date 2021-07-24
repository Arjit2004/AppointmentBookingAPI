using AppointmentBooking.Domain;
using AppointmentBooking.Service;
using AppointmentBookingAPI.Controllers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using Xunit;

namespace AppointmentBooking.Api.Tests
{
    public class AppointmentBookingControllerTests
    {
        [Fact]
        public void EnsureValidRequestWorks()
        {
            var mockService = Substitute.For<IBookingService>();
            mockService.BookAppointmentAsync(Arg.Any<AppointmentBookingRequest>())
                .Returns(new AppointmentBookingResponse() { BookingId = Guid.NewGuid()});

            var logger = Substitute.For<ILogger<AppointmentBookingController>>();

            var controller = new AppointmentBookingController(logger, mockService);
            var result = controller.BookAppointmentAsync(new AppointmentBookingRequest());

            result.Should().NotBeNull();
            ((Microsoft.AspNetCore.Mvc.ObjectResult)result.Result).StatusCode.Should().Be(200);
        }

        [Fact]
        public void EnsureConflictedRequestWorks()
        {
            var mockService = Substitute.For<IBookingService>();
            mockService.BookAppointmentAsync(Arg.Any<AppointmentBookingRequest>())
                .Returns(new AppointmentBookingResponse() { BookingId = Guid.Empty });

            var logger = Substitute.For<ILogger<AppointmentBookingController>>();

            var controller = new AppointmentBookingController(logger, mockService);
            var result = controller.BookAppointmentAsync(new AppointmentBookingRequest());

            result.Should().NotBeNull();
            ((Microsoft.AspNetCore.Mvc.ObjectResult)result.Result).StatusCode.Should().Be(409);
        }
    }
}
