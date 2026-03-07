using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkFlow.Api.Data;
using ParkFlow.Api.DTOs.ParkingSpots;

namespace ParkFlow.Api.Controllers
{
	[ApiController]
    [Route("api/[controller]")]
    public class ParkingSpotController : ControllerBase
    {
		private readonly AppDbContext _context;

        public ParkingSpotController(AppDbContext context)
		{
			_context = context;
		}

        [HttpGet("dashboard")]
		public async Task<IActionResult> GetDashboard()
		{
			var spots = await _context.ParkingSpots
				.Where(s => s.IsActive)
				.Select(s => new DashboardResponse
				{
					ParkingSpotId = s.Id,
					SpotNumber = s.SpotNumber,
					IsOccupied = s.IsOccupied,

					TicketId = s.Tickets
						.Where(t => !t.ExitTime.HasValue)
						.Select(t => (int?)t.Id)
						.FirstOrDefault(),

					LicensePlate = s.Tickets
						.Where(t => !t.ExitTime.HasValue)
						.Select(t => t.Vehicle.LicensePlate)
						.FirstOrDefault(),

					Model = s.Tickets
						.Where(t => !t.ExitTime.HasValue)
						.Select(t => t.Vehicle.Model)
						.FirstOrDefault(),

					Color = s.Tickets
						.Where(t => !t.ExitTime.HasValue)
						.Select(t => t.Vehicle.Color)
						.FirstOrDefault(),

					EntryTime = s.Tickets
						.Where(t => !t.ExitTime.HasValue)
						.Select(t => (DateTime?)t.EntryTime)
						.FirstOrDefault()
				})
				.OrderBy(s => s.SpotNumber)
				.ToListAsync();

			return Ok(spots);
		}
    }
}
