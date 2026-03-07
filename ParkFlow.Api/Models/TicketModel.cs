namespace ParkFlow.Api.Models
{
    public class TicketModel
    {
        public int Id { get; set; }
        public DateTime EntryTime { get; set; } = DateTime.Now;
        public DateTime? ExitTime { get; set; }
        public decimal? TotalAmount { get; set; }
        public bool IsPaid { get; set; } = false;

        public int VehicleId { get; set; }
        public VehicleModel Vehicle { get; set; } = null!;

        public int ParkingSpotId { get; set; }
        public ParkingSpotModel ParkingSpot { get; set; } = null!;
    }
}
