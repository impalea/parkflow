namespace ParkFlow.Api.DTOs.Tickets
{
    public class CheckOutPreviewResponse
    {
        public int TicketId { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
		public string Model { get; set; } = string.Empty;
		public string Color { get; set; } = string.Empty;
        public string SpotNumber { get; set; } = string.Empty;
        public DateTime EntryTime { get; set; }
        public DateTime ExitTime { get; set; }
        public string Duration { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
    }
}
