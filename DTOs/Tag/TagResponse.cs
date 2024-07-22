using LemonLime.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace LemonLime.DTOs.Tag
{
    public class TagResponse : BaseResponse
    {
        [Display(Name = "Tag Name")]
        public string Name { get; set; }
    }
}
