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
using LemonLime.DTOs.Role;
using Microsoft.AspNetCore.Authorization;

namespace LemonLime.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]")]
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RolesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var roles = await _context.Roles.Where(r => r.IsActive).ToListAsync();
            var roleResponses = _mapper.Map<List<RoleResponse>>(roles);

            return View(roleResponses);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id && r.IsActive);

            if (role == null)
            {
                return NotFound();
            }

            var roleResponse = _mapper.Map<RoleResponse>(role);

            return View(roleResponse);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleRequest roleRequest)
        {
            if (ModelState.IsValid)
            {
                var existingRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleRequest.Name && !r.IsActive);

                if (existingRole != null)
                {
                    existingRole.IsActive = true;
                    existingRole.UpdatedTime = DateTime.UtcNow;
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }

                var role = _mapper.Map<Role>(roleRequest);
                role.UpdatedTime = DateTime.UtcNow;

                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(roleRequest);
        }

        [HttpGet("edit/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id && r.IsActive);

            if (role == null)
            {
                return NotFound();
            }

            var roleRequest = _mapper.Map<RoleRequest>(role);

            return View(roleRequest);
        }

        [HttpPost("edit/{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, RoleRequest roleRequest)
        {
            if (ModelState.IsValid)
            {
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id && r.IsActive);

                if (role == null)
                {
                    return NotFound();
                }

                _mapper.Map(roleRequest, role);
                role.UpdatedTime = DateTime.UtcNow;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Roles.Any(r => r.Id == id))
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

            return View(roleRequest);
        }

        [HttpGet("delete/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id && r.IsActive);

            if (role == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<RoleResponse>(role));
        }

        [HttpPost("delete/{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id && r.IsActive);

            if (role == null)
            {
                return NotFound();
            }

            var usersWithRole = await _context.Users.Where(u => u.RoleId == id).ToListAsync();

            if (usersWithRole.Any())
            {
                TempData["ErrorMessage"] = "Bu rolü kullanan kullanıcılar var. Rol silinemiyor.";
                return RedirectToAction(nameof(Index));
            }

            role.IsActive = false;
            role.UpdatedTime = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Roles.Any(r => r.Id == id))
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
    }
}
