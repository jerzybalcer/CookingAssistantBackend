namespace CookingAssistantBackend.Models.DTOs
{
    public class CommentDto
    {
        public CommentDto(int commentId, string commentText, int writtenById, int likesCount, string writtenByName)
        {
            CommentId = commentId;
            CommentText = commentText;
            WrittenById = writtenById;
            LikesCount = likesCount;
            WrittenByName = writtenByName;
        }

        public CommentDto()
        {

        }

        public int CommentId { get; set; }
        public string CommentText { get; set; }
        public int WrittenById { get; set; }
        public string WrittenByName { get; set; }
        public int LikesCount { get; set; }
    }
}
