namespace CookingAssistant.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string CommentText { get; set; }
        public int Likes { get; set; }
        public User User { get; set; }

        public Comment(string commentText, int likes, User user)
        {
            CommentText = commentText;
            Likes = likes;
            User = user;
        }
        private Comment()
        {

        }
    }
}
