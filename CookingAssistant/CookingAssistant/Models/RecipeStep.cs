namespace CookingAssistant.Models
{
    public class RecipeStep
    {
        public string Instruction { get; set; }
        public TimeSpan Time { get; set; }
        
        RecipeStep(string instruction, TimeSpan time)
        {
            Instruction = instruction;
            Time = time;
        }
    }
}
