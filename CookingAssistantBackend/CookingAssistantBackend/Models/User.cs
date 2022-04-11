namespace CookingAssistantBackend.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public ICollection<Recipe> Recipes { get; set; }
        public UserAccount UserAccount { get; set; }
        public int UserAccountId { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
        public User(string name, UserAccount userAccount)
        {
            Name = name;
            UserAccount = userAccount;
            UserAccountId = UserAccount.UserAccountId;

            Recipes = new List<Recipe>();
            Comments = new List<Comment>();
            Likes = new List<Like>();
        }
        private User()
        {

        }
    }
}