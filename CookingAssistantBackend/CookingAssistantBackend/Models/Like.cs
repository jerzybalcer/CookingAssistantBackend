namespace CookingAssistantBackend.Models
{
    public class Like
    {
        public int LikeId { get; set; }
        public Comment Comment { get; set; }
        public User LikedBy { get; set; }

        public Like(Comment comment, User likedBy)
        {
            Comment = comment;
            LikedBy = likedBy;
        }
        private Like()
        {

        }
    }
}
