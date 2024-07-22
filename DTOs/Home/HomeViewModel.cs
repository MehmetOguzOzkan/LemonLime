using LemonLime.DTOs.Recipe;
using LemonLime.DTOs.Tag;
using LemonLime.DTOs.User;

namespace LemonLime.DTOs.Home
{
    public class HomeViewModel
    {
        public List<RecipeHomeResponse> MostLikedRecipes { get; set; }
        public List<UserHomeResponse> MostLikedUsers { get; set; }
        public List<RecipeHomeResponse> RecentRecipes { get; set; }
        public List<TagHomeResponse> TagsWithCounts { get; set; }
    }
}
