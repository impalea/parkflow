using System.ComponentModel.DataAnnotations;

namespace ParkFlow.Api.DTOs.ParkingSpots
{
	public class CreateParkingSpotRequest
	{
		[Required(ErrorMessage = "SpotNumber is required.")]
		public string SpotNumber { get; set; } = string.Empty;
	}
}
