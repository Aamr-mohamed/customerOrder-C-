using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerOrderSystem.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

		public string ProductName { get; set; }

		public string ProductDescription { get; set; }

        public decimal Price { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
		public virtual int OrderId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
		public int ProductId { get; set; }
    }
}

