namespace CookingAssistantBackend.Models.DTOs
{
    public class RecipeDto
    {
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public ICollection<RecipeStepDto> Steps { get; set; }
        public ICollection<RecipeIngredient> Ingredients { get; set; }
        public ICollection<TagDto> Tags { get; set; }
        public RecipeCategory Category { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }

        public RecipeDto()
        {

        }
    }
}
