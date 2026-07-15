using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using StudentManagement.ViewModels;
using Xunit;

namespace StudentManagement.Tests.Api
{
    public class FlightsApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public FlightsApiIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        private static FlightCreateUpdateDto SampleCreateDto(string suffix) => new()
        {
            FlightNumber = $"FL{suffix}",
            Origin = "Manila",
            Destination = "Cebu",
            DepartureDate = DateTime.UtcNow.AddDays(1),
            SeatClass = "Economy"
        };

        [Fact]
        public async Task GET_All_ReturnsOkAndArray()
        {
            await _client.PostAsJsonAsync("/api/flights", SampleCreateDto(Guid.NewGuid().ToString("N")[..6]));

            var response = await _client.GetAsync("/api/flights");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var flights = await response.Content.ReadFromJsonAsync<List<FlightDto>>();
            flights.Should().NotBeNull();
            flights!.Should().NotBeEmpty();
        }

        [Fact]
        public async Task POST_WithValidData_ReturnsCreatedWithLocationHeader()
        {
            var dto = SampleCreateDto(Guid.NewGuid().ToString("N")[..6]);

            var response = await _client.PostAsJsonAsync("/api/flights", dto);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();

            var created = await response.Content.ReadFromJsonAsync<FlightDto>();
            created.Should().NotBeNull();
            created!.FlightNumber.Should().Be(dto.FlightNumber);
            created.FlightId.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public async Task GET_ById_WhenFlightExists_ReturnsOk()
        {
            var dto = SampleCreateDto(Guid.NewGuid().ToString("N")[..6]);
            var createResponse = await _client.PostAsJsonAsync("/api/flights", dto);
            var created = await createResponse.Content.ReadFromJsonAsync<FlightDto>();

            var response = await _client.GetAsync($"/api/flights/{created!.FlightId}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var fetched = await response.Content.ReadFromJsonAsync<FlightDto>();
            fetched!.FlightId.Should().Be(created.FlightId);
        }

        [Fact]
        public async Task PUT_WithValidData_ReturnsOkAndUpdatesFlight()
        {
            var dto = SampleCreateDto(Guid.NewGuid().ToString("N")[..6]);
            var createResponse = await _client.PostAsJsonAsync("/api/flights", dto);
            var created = await createResponse.Content.ReadFromJsonAsync<FlightDto>();

            var updateDto = SampleCreateDto(Guid.NewGuid().ToString("N")[..6]);
            updateDto.Origin = "Davao";

            var response = await _client.PutAsJsonAsync($"/api/flights/{created!.FlightId}", updateDto);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var updated = await response.Content.ReadFromJsonAsync<FlightDto>();
            updated!.Origin.Should().Be("Davao");
        }

        [Fact]
        public async Task DELETE_WhenFlightExists_ReturnsNoContent()
        {
            var dto = SampleCreateDto(Guid.NewGuid().ToString("N")[..6]);
            var createResponse = await _client.PostAsJsonAsync("/api/flights", dto);
            var created = await createResponse.Content.ReadFromJsonAsync<FlightDto>();

            var response = await _client.DeleteAsync($"/api/flights/{created!.FlightId}");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync($"/api/flights/{created.FlightId}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
