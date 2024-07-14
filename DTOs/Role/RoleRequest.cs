using System.ComponentModel.DataAnnotations;

namespace LemonLime.DTOs.Role
{
    public class RoleRequest
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Role Name")]
        public string Name { get; set; }
    }
}
