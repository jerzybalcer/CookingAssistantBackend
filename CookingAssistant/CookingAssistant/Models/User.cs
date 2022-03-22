namespace CookingAssistant.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<Recipe> Recipes { get; set; }

        public User(string name, string email)
        {
            Name = name;
            Email = email;
            
            Recipes = new List<Recipe>();
        }
    }
}
