namespace CookingAssistantBackend.Models
{
    public class RecipeIngredient
    {
        public int RecipeIngredientId { get; set; }
        public string Name { get; set; }
        public int Ammount { get; set; }
        public Units Unit { get; set; }

        public RecipeIngredient(string name, int amount, Units unit)
        {
            Name = name;
            Ammount = amount;
            Unit = unit;
        }
        private RecipeIngredient()
        {

        }
    }
}
