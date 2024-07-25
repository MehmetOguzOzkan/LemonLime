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
using LemonLime.DTOs.Tag;
using LemonLime.DTOs;
using LemonLime.DTOs.User;
using Newtonsoft.Json;

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

        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 10;
            var recipes = _context.Recipes.Where(r => r.IsActive)
                .Include(r => r.Images)
                .Include(r => r.Ratings)
                .Include(r => r.Comments)
                .OrderByDescending(r => r.CreatedTime);

            var recipeDtos = await PaginatedList<RecipeHomeResponse>.CreateAsync(_mapper.ProjectTo<RecipeHomeResponse>(recipes.AsNoTracking()), page ?? 1, pageSize);

            return View(recipeDtos);
        }

        [HttpGet("list")]
        public async Task<IActionResult> List(int? page)
        {
            int pageSize = 10;
            var recipes = _context.Recipes.Where(r => r.IsActive)
                .Include(r => r.Images)
                .Include(r => r.Ratings)
                .Include(r => r.Comments)
                .OrderByDescending(r => r.CreatedTime);

            var recipeDtos = await PaginatedList<RecipeHomeResponse>.CreateAsync(_mapper.ProjectTo<RecipeHomeResponse>(recipes.AsNoTracking()), page ?? 1, pageSize);

            return View(recipeDtos);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var recipe = await _context.Recipes.Where(r => r.IsActive)
                .Include(r => r.CreatedByUser)
                .Include(r => r.NutritionInfo)
                .Include(r => r.Images)
                .Include(r => r.RecipeTags)
                    .ThenInclude(rt => rt.Tag)
                .Include(r => r.Comments)
                    .ThenInclude(c => c.User)
                .Include(r => r.Ratings)
                .FirstOrDefaultAsync(r => r.Id == id && r.IsActive);

            var user = await _context.Users
                .Where(u => u.Id == recipe.CreatedByUser.Id)
                .Include(u => u.Recipes.Where(r => r.IsActive))
                .ThenInclude(r => r.Ratings)
                .OrderByDescending(u => u.Recipes.Average(r => r.Ratings.Any() ? r.Ratings.Average(rat => rat.Value) : 0) * u.Recipes.Count)
                .Take(1)
                .ToListAsync();

            var userResponses = user.Select(u => _mapper.Map<UserHomeResponse>(u)).ToList();


            if (recipe == null)
            {
                return NotFound();
            }

            var recipeResponse = _mapper.Map<RecipeResponse>(recipe);
            recipeResponse.CreatedByUser = userResponses.FirstOrDefault();
            return View(recipeResponse);
        }

        [HttpGet("collections/{userid:guid}")]
        public async Task<IActionResult> GetRecipesByUserId(Guid userId)
        {
            var recipes = await _context.Recipes.Where(r => r.IsActive)
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

        [HttpGet("tag/{tagId:guid}")]
        public async Task<IActionResult> GetRecipesByTagId(Guid tagId)
        {
            var recipesQuery = _context.RecipeTags.Where(r => r.IsActive)
                .Where(rt => rt.TagId == tagId)
                .Select(rt => rt.Recipe)
                .Where(r => r.IsActive)
                .Distinct();

            var recipes = await recipesQuery.Where(r => r.IsActive)
                .Include(r => r.Images)
                .Include(r => r.Ratings)
                .Include(r => r.Comments)
                .Include(r => r.NutritionInfo)
                .Include(r => r.RecipeTags)
                    .ThenInclude(rt => rt.Tag)
                .ToListAsync();

            if (!recipes.Any())
            {
                return View("NoRecipesFound");
            }
            var response = _mapper.Map<List<RecipeHomeResponse>>(recipes);

            ViewBag.TagName = await _context.Tags
                .Where(t => t.Id == tagId && t.IsActive)
                .Select(t => t.Name)
                .FirstOrDefaultAsync();

            return View("TagRecipes", response);
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

            TempData["Tags"] = JsonConvert.SerializeObject(tags);
            TempData.Keep("Tags");

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

        // GET: Recipes/Edit/{id}
        [HttpGet("edit/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var recipe = await _context.Recipes
                .Include(r => r.NutritionInfo)
                .Include(r => r.RecipeTags)
                    .ThenInclude(rt => rt.Tag)
                .FirstOrDefaultAsync(r => r.Id == id && r.IsActive);

            var tags = await _context.Tags.Where(t => t.IsActive).ToListAsync();

            if (recipe == null)
            {
                return NotFound();
            }

            var request = _mapper.Map<RecipeEditRequest>(recipe);

            return View(request);
        }



        // POST: Recipes/Edit/{id}
        [HttpPost("edit/{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [FromForm] RecipeEditRequest recipeRequest)
        {

            if (id == Guid.Empty)
            {
                return BadRequest("Recipe ID is missing");
            }

            var recipe = await _context.Recipes
                .Include(r => r.NutritionInfo)
                .Include(r => r.RecipeTags)
                .FirstOrDefaultAsync(r => r.Id == id);

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
                _mapper.Map(recipeRequest, recipe);

                if (recipeRequest.NutritionInfo != null)
                {
                    if (recipe.NutritionInfo != null)
                    {
                        _mapper.Map(recipeRequest.NutritionInfo, recipe.NutritionInfo);
                    }
                    else
                    {
                        var nutritionInfo = _mapper.Map<NutritionInfo>(recipeRequest.NutritionInfo);
                        recipe.NutritionInfo = nutritionInfo;
                        _context.NutritionInfos.Add(nutritionInfo);
        }

        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = recipe.Id });
            }

            return View(recipeRequest);
        }

        [HttpPost("delete/{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            recipe.IsActive = false;
            _context.Recipes.Update(recipe);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index","Home");
        }

        private bool RecipeExists(Guid id)
        {
            return _context.Recipes.Any(e => e.Id == id);
        }*/
    }
}
