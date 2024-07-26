using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LemonLime.Context;
using LemonLime.Models;
using AutoMapper;
using LemonLime.DTOs.Tag;
using Microsoft.AspNetCore.Authorization;

namespace LemonLime.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]")]
    public class TagsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TagsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var tags = await _context.Tags.Where(t => t.IsActive)
                .OrderBy(t => t.Name)
                .ToListAsync();
            var tagResponses = _mapper.Map<List<TagResponse>>(tags);

            return View(tagResponses);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id && t.IsActive);

            if (tag == null)
            {
                return NotFound();
            }

            var tagResponse = _mapper.Map<TagResponse>(tag);

            return View(tagResponse);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TagRequest tagRequest)
        {
            if (ModelState.IsValid)
            {
                var existingTag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagRequest.Name && !t.IsActive);

                if (existingTag != null)
                {
                    existingTag.IsActive = true;
                    existingTag.UpdatedTime = DateTime.UtcNow;
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }

                var tag = _mapper.Map<Tag>(tagRequest);

                await _context.Tags.AddAsync(tag);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(tagRequest);
        }

        [HttpGet("edit/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id && t.IsActive);

            if (tag == null)
            {
                return NotFound();
            }

            var tagRequest = _mapper.Map<TagRequest>(tag);

            return View(tagRequest);
        }

        [HttpPost("edit/{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, TagRequest tagRequest)
        {
            if (ModelState.IsValid)
            {
                var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id && t.IsActive);

                if (tag == null)
                {
                    return NotFound();
                }

                _mapper.Map(tagRequest, tag);
                tag.UpdatedTime = DateTime.UtcNow;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Tags.Any(t => t.Id == id))
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

            return View(tagRequest);
        }

        [HttpGet("delete/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id && t.IsActive);

            if (tag == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<TagResponse>(tag));
        }

        [HttpPost("delete/{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id && t.IsActive);

            if (tag == null)
            {
                return NotFound();
            }

            var recipesWithTag = await _context.RecipeTags
                .Include(rt => rt.Recipe)
                .Where(rt => rt.TagId == id)
                .ToListAsync();

            if (recipesWithTag.Any())
            {
                TempData["ErrorMessage"] = "This tag cannot be deleted because it belongs to a recipe.";
                return RedirectToAction(nameof(Index));
            }


            tag.IsActive = false;
            tag.UpdatedTime = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Tags.Any(t => t.Id == id))
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
