namespace CustomerOrderSystem.Models
{
	public class OrderResponse
	{
		public int OrderId { get; set; }
		public int UserId { get; set; }
		public int ProductId { get; set; }
		public decimal Price { get; set; }
		public ICollection<OrderItem> OrderItems { get; set; }
	}
}
