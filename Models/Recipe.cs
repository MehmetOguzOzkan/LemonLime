using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LemonLime.Models
{
    public class Recipe : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public string Ingredients { get; set; }

        [Required]
        public string Instructions { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Cooking time must be a positive value.")]
        public int CookingTimeInMinutes { get; set; }

        public NutritionInfo NutritionInfo { get; set; }

        [Required]
        public Guid CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public User CreatedByUser { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<Image> Images { get; set; }
        public ICollection<RecipeTag> RecipeTags { get; set; }

    }
}
