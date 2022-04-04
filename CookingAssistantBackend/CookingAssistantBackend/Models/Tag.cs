namespace CookingAssistantBackend.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        public string Name { get; set; }
        public ICollection<Recipe> Recipes { get; set; }

        public Tag(string name)
        {
            Name = name;
            Recipes = new List<Recipe>();
        }
        private Tag()
        {

        }
    }
}
