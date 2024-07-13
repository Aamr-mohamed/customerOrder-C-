using Swashbuckle.AspNetCore.Annotations;

namespace CustomerOrderSystem.Models
{
    public class OrderRequest
    {
		/// <example>1</example>
        [SwaggerParameter(Description = "The user id of the user who is placing the order")]
        public int UserId { get; set; }

		/// <example>John Doe</example>
        [SwaggerParameter(Description = "The name of the customer placing the order")]
        public string? CustomerName { get; set; }

		/// <example>2024-07-13T10:51:26.748Z</example>
        [SwaggerParameter(Description = "The date the order was placed")]
        public DateTime OrderDate { get; set; }

		/// <example>new[] { new CreateOrderItemRequest { ProductId = 1, Quantity = 2, Price = 10.5 } }</example>
        [SwaggerParameter(Description = "The items in the order")]
        public List<CreateOrderItemRequest> OrderItems { get; set; }
    }

    public class CreateOrderItemRequest
    {
		/// <example>1</example>
        [SwaggerParameter(Description = "The id of the product")]
        public int ProductId { get; set; }
		/// <example>2</example>
        [SwaggerParameter(Description = "The quantity of the product")]
        public int Quantity { get; set; }
		/// <example>10.5</example>
        [SwaggerParameter(Description = "The price of the product")]
        public decimal Price { get; set; }
    }
}


