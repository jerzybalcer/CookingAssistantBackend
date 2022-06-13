namespace CookingAssistantBackend.Models.DTOs
{
    public class NewCommentDto
    {
        public int CommmmentId { get; set; }
        public string CommentText { get; set; }
        public int StepId { get; set; }
        public int WrittenById { get; set; }
        public string WrittenByName { get; set; }
        public NewCommentDto(int commmmentId, string commentText, int stepId, int writtenById, string writtenByName)
        {
            CommmmentId = commmmentId;
            CommentText = commentText;
            StepId = stepId;
            WrittenById = writtenById;
            WrittenByName = writtenByName;
        }

        public NewCommentDto()
        {

        }
    }
}
