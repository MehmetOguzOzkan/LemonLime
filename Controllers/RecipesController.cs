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
using LemonLime.DTOs.Recipe;

namespace LemonLime.Controllers
{
    [Route("[controller]")]
    public class RecipesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RecipesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Recipes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Recipes.Include(r => r.CreatedByUser);
            return View(await applicationDbContext.ToListAsync());
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var recipe = await _context.Recipes
                .Include(r => r.CreatedByUser)
                .Include(r => r.NutritionInfo)
                .Include(r => r.Images)
                .Include(r => r.RecipeTags)
                    .ThenInclude(rt => rt.Tag)
                .Include(r => r.Comments)
                    .ThenInclude(c => c.User)
                .Include(r => r.Ratings)
                .FirstOrDefaultAsync(r => r.Id == id && r.IsActive);

            if (recipe == null)
            {
                return NotFound();
            }

            var recipeResponse = _mapper.Map<RecipeResponse>(recipe);
            return View(recipeResponse);
        }

        [HttpGet("collections/{userid:guid}")]
        public async Task<IActionResult> GetRecipesByUserId(Guid userId)
        {
            var recipes = await _context.Recipes
                .Where(r => r.CreatedBy == userId && r.IsActive)
                .Include(r => r.Images)
                .Include(r => r.Ratings)
                .Include(r => r.Comments)
                .ToListAsync();

            if (!recipes.Any())
            {
                return View("NoRecipesFound");
            }

            var response = recipes.Select(recipe => _mapper.Map<RecipeHomeResponse>(recipe)).ToList();

            return View("UserRecipes",response);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            var tags = _context.Tags
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name
                }).ToList();

            ViewBag.Tags = tags;

            return View();
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] RecipeRequest recipeRequest)
        {
            if (recipeRequest == null)
            {
                return BadRequest("Recipe data is null");
            }

            var userId = new Guid(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            var recipe = _mapper.Map<Recipe>(recipeRequest);
            recipe.CreatedBy = userId;

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            if (recipeRequest.NutritionInfo != null)
            {
                var nutritionInfo = _mapper.Map<NutritionInfo>(recipeRequest.NutritionInfo);
                _context.NutritionInfos.Add(nutritionInfo);
                recipe.NutritionInfo = nutritionInfo;
            }

            if (recipeRequest.Images != null && recipeRequest.Images.Count > 0)
            {
                foreach (var file in recipeRequest.Images)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "recipes", fileName); 
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        var image = new Image
                        {
                            Url = $"img/recipes/{fileName}",
                            RecipeId = recipe.Id
                        };
                        _context.Images.Add(image);
                    }
                }
            }

            if (recipeRequest.TagIds != null && recipeRequest.TagIds.Count > 0)
            {
                foreach (var tagId in recipeRequest.TagIds)
                {
                    var tag = await _context.Tags.FindAsync(tagId);
                    if (tag != null)
                    {
                        var recipeTag = new RecipeTag
                        {
                            RecipeId = recipe.Id,
                            TagId = tagId
                        };
                        _context.RecipeTags.Add(recipeTag);
                    }
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = recipe.Id });
        }

































        /*
        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            ViewData["CreatedBy"] = new SelectList(_context.Users, "Id", "Email", recipe.CreatedBy);
            return View(recipe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Title,Description,Ingredients,Instructions,CookingTimeInMinutes,CreatedBy,Id,CreatedTime,UpdatedTime,IsActive")] Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.Id))
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
            ViewData["CreatedBy"] = new SelectList(_context.Users, "Id", "Email", recipe.CreatedBy);
            return View(recipe);
        }

        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.CreatedByUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe != null)
            {
                _context.Recipes.Remove(recipe);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeExists(Guid id)
        {
            return _context.Recipes.Any(e => e.Id == id);
        }*/
    }
}
