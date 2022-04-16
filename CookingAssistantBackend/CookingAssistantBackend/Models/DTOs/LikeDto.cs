namespace CookingAssistantBackend.Models.DTOs
{
    public class LikeDto
    {
        public LikeDto(int likeId, int commentId, int likedById, string likedByName)
        {
            LikeId = likeId;
            CommentId = commentId;
            LikedById = likedById;
            LikedByName = likedByName;
        }

        public int LikeId { get; set; }
        public int CommentId { get; set; }
        public int LikedById { get; set; }
        public string LikedByName { get; set; }

    }
}
