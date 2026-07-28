using StudentManagement.Interfaces;
using StudentManagement.Models;
using StudentManagement.ViewModels;

namespace StudentManagement.Services
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _repository;

        public FlightService(IFlightRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<FlightDto>> GetAllAsync()
        {
            var flights = await _repository.GetAllAsync();
            return flights.Select(MapToDto);
        }

        public async Task<FlightDto?> GetByIdAsync(Guid id)
        {
            var flight = await _repository.GetByIdAsync(id);
            return flight is null ? null : MapToDto(flight);
        }

        public async Task<(bool Success, FlightDto? Flight, string? ErrorMessage)> CreateAsync(FlightCreateUpdateDto dto)
        {
            var entity = new Flight
            {
                FlightNumber = dto.FlightNumber.Trim(),
                Origin = dto.Origin.Trim(),
                Destination = dto.Destination.Trim(),
                DepartureDate = dto.DepartureDate,
                SeatClass = dto.SeatClass.Trim()
            };

            var created = await _repository.AddAsync(entity);
            return (true, MapToDto(created), null);
        }

        public async Task<(bool Success, FlightDto? Flight, string? ErrorMessage)> UpdateAsync(Guid id, FlightCreateUpdateDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null)
            {
                return (false, null, "Flight not found.");
            }

            existing.FlightNumber = dto.FlightNumber.Trim();
            existing.Origin = dto.Origin.Trim();
            existing.Destination = dto.Destination.Trim();
            existing.DepartureDate = dto.DepartureDate;
            existing.SeatClass = dto.SeatClass.Trim();

            await _repository.UpdateAsync(existing);
            return (true, MapToDto(existing), null);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }

        private static FlightDto MapToDto(Flight flight) => new()
        {
            FlightId = flight.FlightId,
            FlightNumber = flight.FlightNumber,
            Origin = flight.Origin,
            Destination = flight.Destination,
            DepartureDate = flight.DepartureDate,
            SeatClass = flight.SeatClass,
            DateCreated = flight.DateCreated,
            DateUpdated = flight.DateUpdated
        };
    }
}
