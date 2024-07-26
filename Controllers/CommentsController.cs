using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using LemonLime.Models;
using Microsoft.EntityFrameworkCore;
using LemonLime.Context;
using LemonLime.DTOs.Comment;
using Microsoft.AspNetCore.Authorization;

[Route("[controller]")]
public class CommentsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CommentsController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [Authorize]
    [HttpGet("Comments/Add/{recipeId}")]
    public IActionResult AddComment(Guid recipeId)
    {
        return View();
    }

    [Authorize]
    [HttpPost("Comments/Add/{recipeId}")]
    public async Task<IActionResult> AddComment(Guid recipeId, CommentRequest commentRequest)
    {
        if (!ModelState.IsValid)
        {
            return View(commentRequest);
        }

        var userId = new Guid(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);


        var existingComment = await _context.Comments
        .FirstOrDefaultAsync(c => c.RecipeId == recipeId && c.UserId == userId);

        var existingRating = await _context.Ratings
            .FirstOrDefaultAsync(r => r.RecipeId == recipeId && r.UserId == userId);

        if (existingComment != null || existingRating != null)
        {
            ModelState.AddModelError("", "You have already added a comment or rating for this recipe.");
            return RedirectToAction("Details", "Recipes", new { id = recipeId });
        }


        var comment = _mapper.Map<Comment>(commentRequest);
        comment.RecipeId = recipeId;
        comment.UserId = userId;

        var rating = new Rating
        {
            RecipeId = recipeId,
            UserId = userId,
            Value = commentRequest.Rating
        };

        _context.Comments.Add(comment);
        _context.Ratings.Add(rating);

        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Recipes", new { id = recipeId });
    }

    [HttpGet("Comments/{recipeId}")]
    public async Task<IActionResult> GetComments(Guid recipeId)
    {
        var comments = await _context.Comments
                                     .Where(c => c.RecipeId == recipeId)
                                     .Include(c => c.User)
                                     .ToListAsync();

        var commentResponses = _mapper.Map<IEnumerable<CommentResponse>>(comments);

        return View(commentResponses);
    }
}
