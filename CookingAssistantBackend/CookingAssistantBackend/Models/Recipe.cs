namespace CookingAssistantBackend.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public ICollection<RecipeStep> Steps { get; set; }
        public ICollection<RecipeIngredient> Ingredients { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public RecipeCategory Category { get; set; }
        public User User { get; set; }

        public Recipe(string name, User user, RecipeCategory category)
        {
            Name = name;
            User = user;
            Category = category;

            Steps = new List<RecipeStep>();
            Ingredients = new List<RecipeIngredient>();
            Tags = new List<Tag>();
        }
        private Recipe()
        {

        }
    }
}