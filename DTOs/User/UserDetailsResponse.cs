using LemonLime.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace LemonLime.DTOs.User
{
    public class UserDetailsResponse : BaseResponse
    {
        public string Username { get; set; }
        public string Email { get; set; }
        [Display(Name = "Photo")]
        public string ProfilePicture { get; set; }
        public string Role { get; set; }

    }
}
