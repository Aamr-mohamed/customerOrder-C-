using Microsoft.AspNetCore.Mvc;
using CustomerOrderSystem.Models;
using CustomerOrderSystem.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CustomerOrderSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemsController : ControllerBase
    {
        private readonly CustomerOrderSystemContext _context;

        public OrderItemsController(CustomerOrderSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderItems()
        {
            return Ok(await _context.OrderItems.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderItem(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);

            if (orderItem == null)
            {
                return NotFound();
            }

            return Ok(orderItem);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderItem(OrderItem orderItem)
        {
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderItem", new { id = orderItem.OrderItemId }, orderItem);
        }
    }
}
