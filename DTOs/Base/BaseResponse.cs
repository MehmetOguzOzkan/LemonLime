using System.ComponentModel.DataAnnotations;

namespace LemonLime.DTOs.Base
{
    public abstract class BaseResponse
    {
        public Guid Id { get; set; }
        [Display(Name = "Created Time")]
        public DateTime CreatedTime { get; set; }

        [Display(Name = "Updated Time")]
        public DateTime UpdatedTime { get; set; }
    }
}
