using LemonLime.DTOs.Base;

namespace LemonLime.DTOs.User
{
    public class UserResponse : BaseResponse
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public string Role { get; set; }
    }
}
