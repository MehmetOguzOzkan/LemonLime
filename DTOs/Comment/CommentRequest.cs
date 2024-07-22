using System.ComponentModel.DataAnnotations;

namespace LemonLime.DTOs.Comment
{
    public class CommentRequest
    {
        [Required]
        public string Content { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        public Guid RecipeId { get; set; }

    }
}
