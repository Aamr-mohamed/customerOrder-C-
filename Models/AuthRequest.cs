using Swashbuckle.AspNetCore.Annotations;

namespace CustomerOrderSystem.Models
{

    public class AuthRequest
    {
        [SwaggerParameter(Description = "Email of the user")]
        public string? Email { get; set; }

        [SwaggerParameter(Description = "Password of the user")]
		[SwaggerSchema()]
        public string? Password { get; set; }
    }
}
