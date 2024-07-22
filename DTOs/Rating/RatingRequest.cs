using System.ComponentModel.DataAnnotations;

namespace LemonLime.DTOs.Rating
{
    public class RatingRequest
    {
        [Required]
        [Range(1, 5)]
        public int Value { get; set; }

        [Required]
        public Guid RecipeId { get; set; }
    }
}
