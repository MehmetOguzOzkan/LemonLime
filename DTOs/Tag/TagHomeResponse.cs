using LemonLime.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace LemonLime.DTOs.Tag
{
    public class TagHomeResponse : BaseResponse
    {
        [Required]
        public string Name { get; set; }

        public int RecipeCount { get; set; }
    }
}
