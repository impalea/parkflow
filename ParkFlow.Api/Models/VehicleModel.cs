namespace ParkFlow.Api.Models
{
	public class VehicleModel
	{
		public int Id { get; set; }
		public string LicensePlate { get; set; } = "";
		public string Model { get; set; } = "";
		public string Color { get; set; } = "";
	}
}
