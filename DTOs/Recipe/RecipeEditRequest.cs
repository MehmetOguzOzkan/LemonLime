using LemonLime.DTOs.NutritionInfo;
using System.ComponentModel.DataAnnotations;

namespace LemonLime.DTOs.Recipe
{
    public class RecipeEditRequest
    {
        public Guid Id { get; set; }

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
        [Display(Name = "Cooking Time (in minutes)")]
        public int CookingTimeInMinutes { get; set; }

        public NutritionInfoRequest NutritionInfo { get; set; }
    }
}
