namespace ParkFlow.Api.DTOs.Tickets
{
    public class CheckInResponse
	{
		public int TicketId { get; set; }
		public string LicensePlate { get; set; } = string.Empty;
		public string Model { get; set; } = string.Empty;
		public string SpotNumber { get; set; } = string.Empty;
		public DateTime EntryTime { get; set; }
	}
}
