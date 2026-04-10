namespace ParkFlow.Api.Models
{
	public class PriceConfigModel
	{
		public int Id { get; set; }
		public bool IsActive { get; set; }
		public int ToleranceMinutes { get; set; }
		public decimal FirstHourValue { get; set; }
		public decimal AdditionalHourValue { get; set; }
		public decimal DailyValue { get; set; }
		public DateTime LastUpdatedAt { get; set; }
	}
}
