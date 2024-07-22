using System.ComponentModel.DataAnnotations;

namespace LemonLime.DTOs.Tag
{
    public class TagRequest
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Tag Name")]
        public string Name { get; set; }
    }
}
