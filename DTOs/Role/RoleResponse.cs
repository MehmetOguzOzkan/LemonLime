using LemonLime.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace LemonLime.DTOs.Role
{
    public class RoleResponse : BaseResponse
    {
        [Display(Name = "Role Name")]
        public string Name { get; set; }

    }
}
