using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LemonLime.Models
{
    public class NutritionInfo : BaseEntity
    {
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Calories must be a positive value.")]
        public int Calories { get; set; }

        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "Fat must be a positive value.")]
        public float Fat { get; set; }

        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "Carbohydrates must be a positive value.")]
        public float Carbohydrates { get; set; }

        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "Protein must be a positive value.")]
        public float Protein { get; set; }

        [Required]
        public Guid RecipeId { get; set; }

        [ForeignKey("RecipeId")]
        public Recipe Recipe { get; set; }
    }
}
