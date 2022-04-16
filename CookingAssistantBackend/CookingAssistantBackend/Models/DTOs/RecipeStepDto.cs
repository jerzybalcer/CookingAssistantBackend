namespace CookingAssistantBackend.Models.DTOs
{
    public class RecipeStepDto
    {
        public RecipeStepDto(int recipeStepId, string instruction, ICollection<CommentDto> comments, TimeSpan? time = null)
        {
            RecipeStepId = recipeStepId;
            Instruction = instruction;
            Time = time;
            Comments = comments;
        }

        public RecipeStepDto()
        {

        }

        public int RecipeStepId { get; set; }
        public string Instruction { get; set; }
        public TimeSpan? Time { get; set; }
        public ICollection<CommentDto> Comments { get; set; }
    }
}
