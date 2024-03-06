using System.ComponentModel.DataAnnotations;

namespace VisionetDiskon.Models
{
	public class DiscountTransaction
	{
		[Key]
		public Guid Id { get; set; }
		public string TransactionId { get; set; } = string.Empty;
		public string CustomerType {  get; set; } = string.Empty;
		public int RewardPoints { get; set; }
		public decimal TotalBelanja {  get; set; }
		public decimal Discount {  get; set; }
		public decimal TotalBayar { get; set; }
	}
}
