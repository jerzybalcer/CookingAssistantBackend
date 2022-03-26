namespace CookingAssistantBackend.Models
{
    public class UserAccount
    {
        public int UserAccountId { get; set; }
        public string HashedPassword { get; set; }
        public User User { get; set; }

        public UserAccount(string hashedPassword, User user)
        {
            HashedPassword = hashedPassword;
            User = user;
        }
        private UserAccount()
        {

        }
    }
}
