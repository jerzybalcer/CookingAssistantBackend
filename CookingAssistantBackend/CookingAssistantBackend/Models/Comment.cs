namespace CookingAssistantBackend.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string CommentText { get; set; }
        public ICollection<Like> Likes { get; set; }
        public User WrittenBy { get; set; }

        public Comment(string commentText, User writtenBy)
        {
            CommentText = commentText;
            WrittenBy = writtenBy;

            Likes = new List<Like>();
        }
        private Comment()
        {

        }
    }
}
