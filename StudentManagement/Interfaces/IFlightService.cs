using StudentManagement.ViewModels;

namespace StudentManagement.Interfaces
{
    public interface IFlightService
    {
        Task<IEnumerable<FlightDto>> GetAllAsync();
        Task<FlightDto?> GetByIdAsync(Guid id);
        Task<(bool Success, FlightDto? Flight, string? ErrorMessage)> CreateAsync(FlightCreateUpdateDto dto);
        Task<(bool Success, FlightDto? Flight, string? ErrorMessage)> UpdateAsync(Guid id, FlightCreateUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
