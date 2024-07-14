using System.ComponentModel.DataAnnotations;

namespace LemonLime.Models
{
    public class Tag : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public ICollection<RecipeTag> RecipeTags { get; set; }
    }
}
