using CustomerOrderSystem.Enums;
using Microsoft.AspNetCore.Identity;

namespace CustomerOrderSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Role Role { get; set; }
		public ICollection<Order> Order { get; set; }
    }
}
