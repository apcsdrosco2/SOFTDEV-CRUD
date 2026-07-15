using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using StudentManagement.Controllers;
using StudentManagement.Interfaces;
using StudentManagement.ViewModels;
using Xunit;

namespace StudentManagement.Tests.Controllers
{
    public class FlightsControllerTests
    {
        private readonly Mock<IFlightService> _serviceMock;
        private readonly FlightsController _sut;

        public FlightsControllerTests()
        {
            _serviceMock = new Mock<IFlightService>();
            _sut = new FlightsController(_serviceMock.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewWithFlights()
        {
            var flights = new List<FlightDto> { new() { FlightNumber = "FL101" } };
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(flights);

            var result = await _sut.Index();

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeSameAs(flights);
        }

        [Fact]
        public async Task Create_Post_WithValidModel_RedirectsToIndex()
        {
            var model = new FlightCreateUpdateDto
            {
                FlightNumber = "FL202",
                Origin = "Manila",
                Destination = "Cebu",
                DepartureDate = DateTime.UtcNow.AddDays(1),
                SeatClass = "Economy"
            };

            _serviceMock.Setup(s => s.CreateAsync(It.IsAny<FlightCreateUpdateDto>()))
                .ReturnsAsync((true, new FlightDto { FlightNumber = "FL202" }, (string?)null));
            _sut.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(
                new Microsoft.AspNetCore.Http.DefaultHttpContext(),
                Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

            var result = await _sut.Create(model);

            var redirect = result.Should().BeOfType<RedirectToActionResult>().Subject;
            redirect.ActionName.Should().Be(nameof(FlightsController.Index));
        }
    }
}
