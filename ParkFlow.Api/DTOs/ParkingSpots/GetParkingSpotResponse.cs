namespace ParkFlow.Api.DTOs.ParkingSpots
{
	public class GetParkingSpotResponse
	{
		public int Id { get; set; }
		public string SpotNumber { get; set; } = string.Empty;
		public bool IsOccupied { get; set; }
	}
}
