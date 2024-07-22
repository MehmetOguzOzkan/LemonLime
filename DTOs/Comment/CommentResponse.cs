using LemonLime.DTOs.Base;
using LemonLime.DTOs.User;

namespace LemonLime.DTOs.Comment
{
    public class CommentResponse : BaseResponse
    {
        public string Content { get; set; }
        public UserHomeResponse User { get; set; }
    }
}
