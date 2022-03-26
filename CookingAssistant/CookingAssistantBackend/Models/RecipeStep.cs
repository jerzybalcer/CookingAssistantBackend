namespace CookingAssistantBackend.Models
{
    public class RecipeStep
    {
        public int RecipeStepId { get; set; }
        public string Instruction { get; set; }
        public TimeSpan? Time { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public RecipeStep(string instruction, TimeSpan? time = null)
        {
            Instruction = instruction;
            Time = time;

            Comments = new List<Comment>();
        }
    }
}
