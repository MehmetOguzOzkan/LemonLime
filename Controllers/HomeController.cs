using AutoMapper;
using LemonLime.Context;
using LemonLime.DTOs.Home;
using LemonLime.DTOs.Recipe;
using LemonLime.DTOs.Tag;
using LemonLime.DTOs.User;
using LemonLime.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LemonLime.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> _logger, ApplicationDbContext _context, IMapper _mapper)
        {
            this._logger = _logger;
            this._context = _context;
            this._mapper = _mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // 1. Most liked recipes (only active ones)
            var mostLikedRecipes = await _context.Recipes
                .Where(r => r.IsActive) // Filter to include only active recipes
                .Include(r => r.Images)
                .Include(r => r.Comments)
                .Include(r => r.Ratings)
                .OrderByDescending(r => r.Ratings.Average(rat => rat.Value) * r.Ratings.Count)
                .Take(4)
                .ToListAsync();

            var mostLikedRecipeDtos = _mapper.Map<List<RecipeHomeResponse>>(mostLikedRecipes);

            // 2. Most liked users (only active ones)
            var mostLikedUsers = await _context.Users
                .Include(u => u.Recipes.Where(r => r.IsActive))
                .ThenInclude(r => r.Ratings)
                .OrderByDescending(u => u.Recipes.Average(r => r.Ratings.Any() ? r.Ratings.Average(rat => rat.Value) : 0) * u.Recipes.Count)
                .Take(2)
                .ToListAsync();

            var mostLikedUserDtos = _mapper.Map<List<UserHomeResponse>>(mostLikedUsers);

            // 3. Recently published recipes (paginated) (only active ones)
            var recentRecipes = await _context.Recipes
                .Where(r => r.IsActive) // Filter to include only active recipes
                .Include(r => r.Images)
                .Include(r => r.Comments)
                .Include(r => r.Ratings)
                .OrderByDescending(r => r.CreatedTime)
                .Take(12)
                .ToListAsync();

            var recentRecipeDtos = _mapper.Map<List<RecipeHomeResponse>>(recentRecipes);

            // 4. All tags with recipe counts
            var tagsWithCounts = await _context.Tags
                .Where(t => t.IsActive)
                .Select(t => new
                {
                    Tag = t,
                    RecipeCount = t.RecipeTags
                        .Where(rt => rt.Recipe.IsActive) // Filter to include only active recipes
                        .Count()
                })
                .OrderBy(t => t.Tag.Name)
                .ToListAsync();

            var tagDtos = tagsWithCounts.Select(tc => new TagHomeResponse
            {
                Id = tc.Tag.Id,
                Name = tc.Tag.Name,
                RecipeCount = tc.RecipeCount
            }).ToList();

            var viewModel = new HomeViewModel
            {
                MostLikedRecipes = mostLikedRecipeDtos,
                MostLikedUsers = mostLikedUserDtos,
                RecentRecipes = recentRecipeDtos,
                TagsWithCounts = tagDtos
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
