using StudentManagement.Models;

namespace StudentManagement.Interfaces
{
    public interface IFlightRepository
    {
        Task<IEnumerable<Flight>> GetAllAsync();
        Task<Flight?> GetByIdAsync(Guid id);
        Task<Flight> AddAsync(Flight flight);
        Task UpdateAsync(Flight flight);
        Task<bool> DeleteAsync(Guid id);
    }
}
