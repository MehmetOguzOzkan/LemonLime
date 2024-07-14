using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LemonLime.Models
{
    public class RecipeTag : BaseEntity
    {
        [Required]
        public Guid RecipeId { get; set; }

        [ForeignKey("RecipeId")]
        public Recipe Recipe { get; set; }

        [Required]
        public Guid TagId { get; set; }

        [ForeignKey("TagId")]
        public Tag Tag { get; set; }
    }
}
