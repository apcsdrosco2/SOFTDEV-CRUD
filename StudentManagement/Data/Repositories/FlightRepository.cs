using Microsoft.EntityFrameworkCore;
using StudentManagement.Interfaces;
using StudentManagement.Models;

namespace StudentManagement.Data.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly ApplicationDbContext _context;

        public FlightRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Flight>> GetAllAsync()
        {
            return await _context.Flights
                .Where(f => f.IsActive)
                .AsNoTracking()
                .OrderBy(f => f.DepartureDate)
                .ToListAsync();
        }

        public async Task<Flight?> GetByIdAsync(Guid id)
        {
            return await _context.Flights
                .FirstOrDefaultAsync(f => f.FlightId == id && f.IsActive);
        }

        public async Task<Flight> AddAsync(Flight flight)
        {
            flight.DateCreated = DateTime.UtcNow;
            flight.IsActive = true;
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();
            return flight;
        }

        public async Task UpdateAsync(Flight flight)
        {
            flight.DateUpdated = DateTime.UtcNow;
            _context.Flights.Update(flight);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var flight = await _context.Flights.FirstOrDefaultAsync(f => f.FlightId == id && f.IsActive);
            if (flight is null)
            {
                return false;
            }

            flight.IsActive = false;
            flight.DateUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
