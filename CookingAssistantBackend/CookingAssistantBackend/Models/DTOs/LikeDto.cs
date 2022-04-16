namespace CookingAssistantBackend.Models.DTOs
{
    public class LikeDto
    {
        public LikeDto(int likeId, int commentId, int likedById)
        {
            LikeId = likeId;
            CommentId = commentId;
            LikedById = likedById;
        }

        public int LikeId { get; set; }
        public int CommentId { get; set; }
        public int LikedById { get; set; }

    }
}
