using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CustomerOrderSystem.Models;
using CustomerOrderSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrderSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly CustomerOrderSystemContext _context;

        public UserController(CustomerOrderSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Policy = "Sales")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _context.User.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var customer = await _context.User.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }
            //check if the id is a number
            if (!int.TryParse(id.ToString(), out int idNumber))
            {
                return BadRequest();
            }

            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(User User)
        {
            if (await _context.User.AnyAsync(c => c.Email == User.Email))
            {
                return BadRequest("Email already exists");
            }
            _context.User.Add(User);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = User.UserId }, User);
        }
    }
}
