namespace ParkFlow.Api.DTOs.ParkingSpots
{
    public class DashboardResponse
    {
		public int ParkingSpotId { get; set; }
		public string SpotNumber { get; set; } = "";
		public bool IsOccupied { get; set; }

		public int? TicketId { get; set; }
		public string? LicensePlate { get; set; }
		public string? Model { get; set; }
		public string? Color { get; set; }
		public DateTime? EntryTime { get; set; }
    }
}
