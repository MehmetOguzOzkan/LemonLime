using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LemonLime.Models
{
    public class Rating : BaseEntity
    {
        [Required]
        [Range(1, 5, ErrorMessage = "Rating value must be between 1 and 5.")]
        public int Value { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public Guid RecipeId { get; set; }

        [ForeignKey("RecipeId")]
        public Recipe Recipe { get; set; }
    }
}
