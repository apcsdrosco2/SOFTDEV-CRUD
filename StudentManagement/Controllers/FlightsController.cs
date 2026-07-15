using Microsoft.AspNetCore.Mvc;
using StudentManagement.Interfaces;
using StudentManagement.ViewModels;

namespace StudentManagement.Controllers
{
    public class FlightsController : Controller
    {
        private readonly IFlightService _flightService;

        public FlightsController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        public async Task<IActionResult> Index()
        {
            var flights = await _flightService.GetAllAsync();
            return View(flights);
        }

        public IActionResult Create()
        {
            return View(new FlightCreateUpdateDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FlightCreateUpdateDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (success, created, error) = await _flightService.CreateAsync(model);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, error ?? "Unable to create flight.");
                return View(model);
            }

            TempData["SuccessMessage"] = $"Flight '{created!.FlightNumber}' was created successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var flight = await _flightService.GetByIdAsync(id);
            if (flight is null)
            {
                return NotFound();
            }

            return View(new FlightCreateUpdateDto
            {
                FlightNumber = flight.FlightNumber,
                Origin = flight.Origin,
                Destination = flight.Destination,
                DepartureDate = flight.DepartureDate,
                SeatClass = flight.SeatClass
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, FlightCreateUpdateDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (success, updated, error) = await _flightService.UpdateAsync(id, model);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, error ?? "Unable to update flight.");
                return View(model);
            }

            TempData["SuccessMessage"] = $"Flight '{updated!.FlightNumber}' was updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var flight = await _flightService.GetByIdAsync(id);
            if (flight is null)
            {
                return NotFound();
            }

            return View(flight);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var deleted = await _flightService.DeleteAsync(id);
            if (!deleted)
            {
                TempData["ErrorMessage"] = "Flight could not be found or was already deleted.";
            }
            else
            {
                TempData["SuccessMessage"] = "Flight was deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
