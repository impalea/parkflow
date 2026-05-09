using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkFlow.Api.Data;
using ParkFlow.Api.DTOs.ParkingSpots;
using ParkFlow.Api.Models;

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

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var spots = await _context.ParkingSpots
				.Where(s => s.IsActive)
				.OrderBy(s => s.SpotNumber)
				.Select(s => new GetParkingSpotResponse
				{
					Id = s.Id,
					SpotNumber = s.SpotNumber,
					IsOccupied = s.IsOccupied
				})
				.OrderBy(s => s.SpotNumber)
				.ToListAsync();

			return Ok(spots);
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateParkingSpotRequest request)
		{
			var existingSpot = await _context.ParkingSpots
				.FirstOrDefaultAsync(s => s.SpotNumber.ToLower() == request.SpotNumber.ToLower());

			if (existingSpot != null)
			{
				if (existingSpot.IsActive)
					return BadRequest(new { Message = "There is already an active parking spot with this number." });

				existingSpot.IsActive = true;

				_context.Entry(existingSpot).State = EntityState.Modified;
				await _context.SaveChangesAsync();

				return Ok(existingSpot);
			}

			var spot = new ParkingSpotModel
			{
				SpotNumber = request.SpotNumber,
				IsOccupied = false,
				IsActive = true
			};

			_context.ParkingSpots.Add(spot);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetAll), new { id = spot.Id }, spot);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var spot = await _context.ParkingSpots.FindAsync(id);

			if (spot == null) return NotFound(new { Message = "Parking spot not found." });

			if (spot.IsOccupied)
				return BadRequest(new { Message = "It is not possible to remove a spot that is currently occupied." });

			spot.IsActive = false;

			_context.Entry(spot).State = EntityState.Modified;
			await _context.SaveChangesAsync();

			return NoContent();
		}
	}
}
