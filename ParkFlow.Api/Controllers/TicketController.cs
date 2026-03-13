using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkFlow.Api.Data;
using ParkFlow.Api.DTOs.Tickets;
using ParkFlow.Api.Models;

namespace ParkFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
	{
		private readonly AppDbContext _context;

		public TicketController(AppDbContext context)
		{
			_context = context;
		}

		[HttpPost("checkin")]
		public async Task<IActionResult> CreateCheckIn([FromBody] CheckInRequest request)
		{
			using var transaction = await _context.Database.BeginTransactionAsync();

			try
			{
				string cleanPlate = System.Text.RegularExpressions.Regex
					.Replace(request.LicensePlate, @"[^a-zA-Z0-9]", "")
					.ToUpper();

				var vehicle = await _context.Vehicles
					.FirstOrDefaultAsync(v => v.LicensePlate == cleanPlate);

				if (vehicle == null)
				{
					vehicle = new VehicleModel
					{
						LicensePlate = cleanPlate,
						Model = request.Model,
						Color = request.Color
					};
					_context.Vehicles.Add(vehicle);
				}
				else
				{
					var alreadyInThePark = await _context.Tickets
						.AnyAsync(t => t.VehicleId == vehicle.Id && !t.ExitTime.HasValue);

        			if (alreadyInThePark) return BadRequest(new { Message = "This vehicle already has an active check-in." });

					vehicle.Model = request.Model;
					vehicle.Color = request.Color;
					_context.Vehicles.Update(vehicle);
				}

				await _context.SaveChangesAsync();

				ParkingSpotModel? spot;
				if (request.ParkingSpotId.HasValue && request.ParkingSpotId.Value > 0)
				{
					spot = await _context.ParkingSpots
						.FindAsync(request.ParkingSpotId.Value);

					if (spot == null) return NotFound(new { Message = "The selected parking spot does not exist." });
					if (spot.IsOccupied || !spot.IsActive) return BadRequest(new { Message = "The selected parking spot is not available." });
				}
				else
				{
					spot = await _context.ParkingSpots
						.OrderBy(s => s.SpotNumber)
						.FirstOrDefaultAsync(s => !s.IsOccupied && s.IsActive);

					if (spot == null) return BadRequest(new { Message = "There are no available parking spots." });
				}

				var ticket = new TicketModel
				{
					VehicleId = vehicle.Id,
					ParkingSpotId = spot.Id
				};

				spot.IsOccupied = true;
				_context.ParkingSpots.Update(spot);

				_context.Tickets.Add(ticket);

				await _context.SaveChangesAsync();
				await transaction.CommitAsync();

				var response = new CheckInResponse
				{
					TicketId = ticket.Id,
					LicensePlate = vehicle.LicensePlate,
					Model = vehicle.Model,
					SpotNumber = spot.SpotNumber,
					EntryTime = ticket.EntryTime
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				return StatusCode(500, $"An error occurred: {ex.Message}");
			}
		}

		[HttpGet("checkout/preview/{id}")]
		public async Task<IActionResult> GetCheckOutPreview(int id)
		{
			var ticket = await _context.Tickets
				.Include(t => t.Vehicle)
				.Include(t => t.ParkingSpot)
				.FirstOrDefaultAsync(t => t.Id == id && !t.ExitTime.HasValue);

			if (ticket == null) return NotFound(new { Message = "Active ticket not found." });

			if (ticket.Vehicle == null || ticket.ParkingSpot == null)
				return StatusCode(500, new { Message = "Internal consistency error: Vehicle or Spot data missing." });

			var now = DateTime.Now;
			TimeSpan duration = now - ticket.EntryTime;

			var response = new CheckOutPreviewResponse
			{
				TicketId = ticket.Id,
				LicensePlate = ticket.Vehicle.LicensePlate,
				Model = ticket.Vehicle.Model,
				Color = ticket.Vehicle.Color,
				SpotNumber = ticket.ParkingSpot.SpotNumber,
				EntryTime = ticket.EntryTime,
				ExitTime = now,
				Duration = $"{(int)duration.TotalHours:D2}:{duration.Minutes:D2}h",
				TotalAmount = 0
			};

			return Ok(response);
		}

		[HttpPost("checkout/{id}")]
		public async Task<IActionResult> ConfirmCheckOut(int id)
		{
			using var transaction = await _context.Database.BeginTransactionAsync();

			try
			{
				var ticket = await _context.Tickets
					.Include(t => t.ParkingSpot)
					.FirstOrDefaultAsync(t => t.Id == id && !t.ExitTime.HasValue);

				if (ticket == null) return BadRequest(new { Message = "Ticket already finalized or not found." });

				ticket.ExitTime = DateTime.Now;
				ticket.IsPaid = true;

				if (ticket.ParkingSpot != null)
        		{
					ticket.ParkingSpot.IsOccupied = false;
					_context.ParkingSpots.Update(ticket.ParkingSpot);
				}
				else
				{
					return StatusCode(500, new { Message = "Internal error: Parking spot reference missing." });
				}

				await _context.SaveChangesAsync();
				await transaction.CommitAsync();

				return Ok(new { Message = "Checkout completed successfully." });
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				return StatusCode(500, $"An error occurred: {ex.Message}");
			}
		}
	}
}
