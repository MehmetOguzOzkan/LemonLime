using LemonLime.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace LemonLime.DTOs.Recipe
{
    public class RecipeHomeResponse : BaseResponse
    {
        [Required]
        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public double AverageRating { get; set; }

        public int CommentCount { get; set; }
        public bool IsActive { get; set; }
    }
}
