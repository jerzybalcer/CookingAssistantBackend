namespace CookingAssistant.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public ICollection<RecipeStep> Steps{ get; set; }
        public ICollection<RecipeIngredient> Ingredients { get; set; }
        public User User { get; set; }

        public Recipe(string name, User user)
        {
            Name = name;
            User = user;

            Steps = new List<RecipeStep>();
            Ingredients = new List<RecipeIngredient>();
        }
    }
}
