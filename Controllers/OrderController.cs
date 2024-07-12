using Microsoft.AspNetCore.Mvc;
using CustomerOrderSystem.Models;
using CustomerOrderSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrderSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly CustomerOrderSystemContext _context;

        public OrderController(CustomerOrderSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            return Ok(await _context.Orders.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }

		[HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return Ok(order);
        }
    }
}
