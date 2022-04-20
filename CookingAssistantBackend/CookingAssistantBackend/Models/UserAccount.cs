namespace CookingAssistantBackend.Models
{
    public class UserAccount
    {
        public int UserAccountId { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public User User { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiries { get; set; }

        public UserAccount(string email, string hashedPassword)
        {
            Email = email;
            HashedPassword = hashedPassword;
            RefreshToken = "token";
            RefreshTokenExpiries = DateTime.MinValue;
        }
        public UserAccount()
        {
            Email = null;
            HashedPassword = null;
        }
    }
}