namespace CookingAssistant.Model
{
    public class RecipeIngredient
    {
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public int Ammount { get; set; }
        public Units Unit { get; set; }
    }
}
