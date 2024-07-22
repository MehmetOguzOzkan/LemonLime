using System.ComponentModel.DataAnnotations;

namespace LemonLime.DTOs.User
{
    public class UserRequest
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Profile Photo")]
        public IFormFile ProfilePicture { get; set; }
    }
}
