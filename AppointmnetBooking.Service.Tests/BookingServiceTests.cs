using AppointmentBooking.Data.Database;
using AppointmentBooking.Domain;
using AppointmentBooking.Domain.Entities;
using AppointmentBooking.Service;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using Xunit;

namespace AppointmnetBooking.Service.Tests
{
    public class BookingServiceTests
    {
        private readonly DbContextOptions<BookingContext> _options;
        private readonly ILogger<BookingService> _mockLogger;
        private readonly BookingContext _mockDBContext;
        private readonly BookingService _service;

        public BookingServiceTests()
        {
            _options = new DbContextOptionsBuilder<BookingContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

            _mockLogger = Substitute.For<ILogger<BookingService>>();
            _mockDBContext = new BookingContext(_options);

            _service = new BookingService(_mockLogger, _mockDBContext);
        }

        [Fact]
        public async void EnsureFirstBookingCreatedSuccessfully()
        {
            var request = new AppointmentBookingRequest()
            {
                BookingTime = "09:30",
                Name = "test"
            };

            var response = await _service.BookAppointmentAsync(request);

            response.Should().NotBeNull();
            response.BookingId.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public async void EnsureConflictedBookingIsRejected()
        {
            CreateSeedData();

            var request = new AppointmentBookingRequest()
            {
                BookingTime = "09:30",
                Name = "test"
            };

            var response = await _service.BookAppointmentAsync(request);

            response.Should().NotBeNull();
            response.BookingId.Should().Be(Guid.Empty);
        }

        private void CreateSeedData()
        {
            _mockDBContext.Booking.Add(new Booking() { BookingTime = new TimeSpan(9, 0, 0), Name = "Person 1" });
            _mockDBContext.Booking.Add(new Booking() { BookingTime = new TimeSpan(9, 0, 0), Name = "Person 2" });
            _mockDBContext.Booking.Add(new Booking() { BookingTime = new TimeSpan(9, 0, 0), Name = "Person 3" });
            _mockDBContext.Booking.Add(new Booking() { BookingTime = new TimeSpan(9, 0, 0), Name = "Person 4" });

            _mockDBContext.SaveChanges();
        }
    }
}
