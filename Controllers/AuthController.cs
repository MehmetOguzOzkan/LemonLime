using AutoMapper;
using LemonLime.Context;
using LemonLime.DTOs.Auth;
using LemonLime.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LemonLime.Controllers
{
    [AllowAnonymous]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AuthController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
                return View(registerRequest);

            var existingUser = await _context.Users
                .AnyAsync(u => u.Email == registerRequest.Email || u.Username == registerRequest.Username);

            if (existingUser)
            {
                ModelState.AddModelError("", "Username or email already in use.");
                return View(registerRequest);
            }

            var user = _mapper.Map<User>(registerRequest);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);
            user.RoleId = (await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User"))?.Id ?? Guid.Empty;
            user.ProfilePicture = await SaveProfilePicture(registerRequest.ProfilePicture);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
                return View(loginRequest);

            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Username == loginRequest.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(loginRequest);
            }

            await SignInUser(user);
            return RedirectToAction("Index", "Home");
        }

        private async Task<string> SaveProfilePicture(IFormFile profilePicture)
        {
            if (profilePicture == null || profilePicture.Length == 0)
                return "/img/profiles/default.png";

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(profilePicture.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "profiles", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profilePicture.CopyToAsync(stream);
            }

            return $"/img/profiles/{fileName}";
        }

        private async Task SignInUser(User user)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == user.RoleId);
            if (role == null)
            {
                throw new Exception("User role not found.");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, role.Name)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        }
    }
}