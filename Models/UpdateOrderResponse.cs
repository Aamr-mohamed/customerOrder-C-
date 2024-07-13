namespace CustomerOrderSystem.Models
{
	public class UpdateOrderResponse
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public string CustomerName { get; set; }
		public DateTime OrderDate { get; set; }
		public ICollection<OrderItem> OrderItems { get; set; }
	}
	public class UpdateOrderItemResponse
	{
		public int Id { get; set; }
		public int OrderId { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
	}
}
