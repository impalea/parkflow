using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkFlow.Api.Data;
using ParkFlow.Api.DTOs.PriceConfigs;
using ParkFlow.Api.Models;

namespace ParkFlow.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class PriceConfigController : ControllerBase
	{
		private readonly AppDbContext _context;

		public PriceConfigController(AppDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> GetConfig()
		{
			var config = await _context.PriceConfigs.FirstOrDefaultAsync(p => p.Id == 1);

			if (config == null) return NotFound(new { Message = "Price config not found." });

			var response = new PriceConfigResponse
			{
				Id = config.Id,
				IsActive = config.IsActive,
				ToleranceMinutes = config.ToleranceMinutes,
				FirstHourValue = config.FirstHourValue,
				AdditionalHourValue = config.AdditionalHourValue,
				DailyValue = config.DailyValue,
				LastUpdatedAt = config.LastUpdatedAt
			};

			return Ok(response);
		}

		[HttpPut]
		public async Task<IActionResult> UpdateConfig([FromBody] PriceConfigRequest request)
		{
			var config = await _context.PriceConfigs.FirstOrDefaultAsync(p => p.Id == 1);

			if (config == null) return NotFound(new { Message = "Price config not found." });

			config.IsActive = request.IsActive;
			config.ToleranceMinutes = request.ToleranceMinutes;
			config.FirstHourValue = request.FirstHourValue;
			config.AdditionalHourValue = request.AdditionalHourValue;
			config.DailyValue = request.DailyValue;
			config.LastUpdatedAt = DateTime.Now;

			await _context.SaveChangesAsync();
			return NoContent();
		}
	}
}
