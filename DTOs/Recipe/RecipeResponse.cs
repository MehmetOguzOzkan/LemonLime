using LemonLime.DTOs.Base;
using LemonLime.DTOs.Comment;
using LemonLime.DTOs.Image;
using LemonLime.DTOs.NutritionInfo;
using LemonLime.DTOs.Rating;
using LemonLime.DTOs.Tag;
using LemonLime.DTOs.User;

namespace LemonLime.DTOs.Recipe
{
    public class RecipeResponse : BaseResponse
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Ingredients { get; set; }
        public string Instructions { get; set; }
        public int CookingTimeInMinutes { get; set; }
        public float AverageRating { get; set; }
        public UserHomeResponse CreatedByUser { get; set; }
        public NutritionInfoResponse NutritionInfo { get; set; }
        public List<ImageResponse> Images { get; set; }
        public List<TagResponse> Tags { get; set; }
        public List<CommentResponse> Comments { get; set; }
        public List<RatingResponse> Ratings { get; set; }
    }
}
