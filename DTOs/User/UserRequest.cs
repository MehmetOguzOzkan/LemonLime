using System.ComponentModel.DataAnnotations;

namespace LemonLime.DTOs.User
{
    public class UserRequest
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public IFormFile ProfilePicture { get; set; }
    }
}
