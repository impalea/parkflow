namespace ParkFlow.Api.DTOs.PriceConfigs
{
    public class PriceConfigRequest
    {
        public bool IsActive { get; set; }
        public int ToleranceMinutes { get; set; }
        public decimal FirstHourValue { get; set; }
        public decimal AdditionalHourValue { get; set; }
        public decimal DailyValue { get; set; }
    }
}
