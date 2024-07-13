using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerOrderSystem.Models
{
    public class Order
    {
		/// <example>1</example>
        public int Id { get; set; }

		/// <example>John Doe</example>
		// [Required]
        public string? CustomerName { get; set; }
        // public int NumberOfItems { get; set; }

		/// <example>2021-01-01T00:00:00</example>
        public DateTime OrderDate { get; set; }

		[ForeignKey("UserId")]
		public virtual ApplicationUser User{ get; set; }
		/// <example>1</example>
		public string UserId { get; set; }

        /// <example>new[] { new OrderItem { ProductId = 1, Quantity = 2, Price = 10.5 } }</example>
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
