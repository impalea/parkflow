namespace ParkFlow.Api.Models
{
    public class ParkingSpotModel
    {
        public int Id { get; set; }
        public string SpotNumber { get; set; } = "";
        public bool IsOccupied { get; set; } = false;
        public bool IsActive { get; set; } = true;
		public virtual ICollection<TicketModel> Tickets { get; set; } = new List<TicketModel>();
    }
}
