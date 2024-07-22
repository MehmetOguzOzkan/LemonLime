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

        /*[HttpGet("{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var user = await _context.Users.Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);

            if (user == null)
                return NotFound();

            return View(_mapper.Map<UserResponse>(user));
        }*/

        [HttpGet("profile/{id:guid}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _context.Users
                .Include(u => u.Recipes)
                .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);

            if (user == null)
                return NotFound();

            var userProfileResponse = _mapper.Map<UserProfileResponse>(user);
            userProfileResponse.RecipeCount = user.Recipes.Count;

            return View("Profile",userProfileResponse);
        }

        [HttpGet("edit/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.IsActive);
            if (user == null)
                return NotFound();

            var userRequest = _mapper.Map<UserRequest>(user);
            return View(userRequest);
        }

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
