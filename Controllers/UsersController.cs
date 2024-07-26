using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LemonLime.Context;
using LemonLime.Models;
using AutoMapper;
using LemonLime.DTOs.User;
using Microsoft.AspNetCore.Authorization;

namespace LemonLime.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UsersController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users
                .Where(u => u.IsActive)
                .Include(u => u.Role)
                .ToListAsync();

            var userResponses = _mapper.Map<List<UserDetailsResponse>>(users);
            return View(userResponses);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);

            if (user == null)
                return NotFound();

            var userDetailsResponse = _mapper.Map<UserDetailsResponse>(user);
            return View(userDetailsResponse);
        }

        [Authorize]
        [HttpGet("profile/{id:guid}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _context.Users
                .Include(u => u.Recipes.Where(r=>r.IsActive))
                .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);

            if (user == null)
                return NotFound();

            var userProfileResponse = _mapper.Map<UserProfileResponse>(user);
            userProfileResponse.RecipeCount = user.Recipes.Where(r=>r.IsActive).Count();

            return View("Profile",userProfileResponse);
        }

        [Authorize]
        [HttpGet("edit/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.IsActive);
            if (user == null)
                return NotFound();

            var userRequest = _mapper.Map<UserRequest>(user);
            return View(userRequest);
        }

        [Authorize]
        [HttpPost("edit/{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UserRequest userRequest)
        {
            if (!ModelState.IsValid)
                return View(userRequest);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.IsActive);
            if (user == null)
                return NotFound();

            user.Username = userRequest.Username;
            user.Email = userRequest.Email;

            if (userRequest.ProfilePicture != null)
                user.ProfilePicture = await SaveProfilePicture(userRequest.ProfilePicture);

            user.UpdatedTime = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("delete/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Recipes.Where(r=>r.IsActive))
                .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);

            if (user == null)
                return NotFound();

            if (user.Recipes.Any())
            {
                TempData["ErrorMessage"] = "This user has recipes. Cannot delete user.";
                return RedirectToAction(nameof(Index));
            }

            return View(_mapper.Map<UserDetailsResponse>(user));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("delete/{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Recipes.Where(r => r.IsActive))
                .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);

            if (user == null)
                return NotFound();

            if (user.Recipes.Any())
            {
                TempData["ErrorMessage"] = "This user has recipes. Cannot delete user.";
                return RedirectToAction(nameof(Index));
            }

            user.IsActive = false;
            user.UpdatedTime = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(u => u.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
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
    }
}
