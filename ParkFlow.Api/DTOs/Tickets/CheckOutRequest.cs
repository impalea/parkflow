using System.ComponentModel.DataAnnotations;

namespace ParkFlow.Api.DTOs.Tickets
{
	public class CheckOutRequest
	{
		[Required(ErrorMessage = "Exit time is required.")]
		public DateTime? ExitTime { get; set; }
	}
}
