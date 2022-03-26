namespace CookingAssistantBackend.Models
{
    public class UserAccount
    {
        public int UserAccountId { get; set; }
        public string HashedPassword { get; set; }
        public string Email { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }

        public UserAccount(string email, string hashedPassword, User user)
        {
            Email = email;
            HashedPassword = hashedPassword;
            User = user;
            UserId = user.UserId;
        }
        private UserAccount()
        {

        }
    }
}
