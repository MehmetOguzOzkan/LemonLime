using LemonLime.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace LemonLime.DTOs.User
{
    public class UserHomeResponse : BaseResponse
    {
        [Required]
        public string Username { get; set; }

        public string ProfilePicture { get; set; }

        public int RecipeCount { get; set; }

        public double AverageRating { get; set; }
    }
}
