using LemonLime.DTOs.Base;

namespace LemonLime.DTOs.User
{
    public class UserProfileResponse : BaseResponse
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public int RecipeCount { get; set; }
    }
}
