using CustomerOrderSystemContext.Data;
using CustomerOrderSystem.Enums;
using CustomerOrderSystem.Models;
using CustomerOrderSystem.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CustomerOrderSystem.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly TokenService _tokenService;

    public UsersController(UserManager<ApplicationUser> userManager, ApplicationDbContext context,
        TokenService tokenService, ILogger<UsersController> logger)
    {
        _userManager = userManager;
        _context = context;
        _tokenService = tokenService;
    }


    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegistrationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _userManager.CreateAsync(
                new ApplicationUser { UserName = request.Username, Email = request.Email, Role = request.Role },
                request.Password!
                );

        if (result.Succeeded)
        {
            request.Password = "";
            return CreatedAtAction(nameof(Register), new { email = request.Email, role = Role.Customer }, request);
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }

        return BadRequest(ModelState);
    }


    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var managedUser = await _userManager.FindByEmailAsync(request.Email!);

        if (managedUser == null)
        {
            return BadRequest("Bad credentials");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password!);

        if (!isPasswordValid)
        {
            return BadRequest("Bad credentials");
        }

        var userInDb = _context.Users.FirstOrDefault(u => u.Email == request.Email);

        if (userInDb is null)
        {
            return Unauthorized();
        }

        var accessToken = _tokenService.CreateToken(userInDb);
        await _context.SaveChangesAsync();

        return Ok(new AuthResponse
        {
            Id = userInDb.Id,
            Username = userInDb.UserName,
            Email = userInDb.Email,
            Token = accessToken,
            Role = userInDb.Role.ToString(),
            Expiration = DateTime.UtcNow.AddMinutes(60).ToString()
        });
    }
}

