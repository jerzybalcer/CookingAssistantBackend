namespace CookingAssistant.Models
{
    public class UserAccount
    {
        public string HashedPassword { get; set; }
        public User User { get; set; }
        
        public UserAccount(string hashedPassword, User user)
        {
            HashedPassword = hashedPassword;
            User = user;
        }
    }
}
