using LemonLime.DTOs.Base;
using LemonLime.DTOs.User;

namespace LemonLime.DTOs.Rating
{
    public class RatingResponse : BaseResponse
    {
        public int Value { get; set; }
        public UserHomeResponse User { get; set; }
    }
}
