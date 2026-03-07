using System.ComponentModel.DataAnnotations;

namespace ParkFlow.Api.DTOs.Tickets
{
    public class CheckInRequest
	{
		[Required(ErrorMessage = "License plate is required.")]
		[StringLength(8, MinimumLength = 7, ErrorMessage = "License plate must have 7-8 characters.")]
		public string LicensePlate { get; set; } = string.Empty;

		[Required(ErrorMessage = "Model is required.")]
		[StringLength(50)]
		public string Model { get; set; } = string.Empty;

		[StringLength(20)]
		public string Color { get; set; } = string.Empty;

		public int? ParkingSpotId { get; set; }
	}
}
