namespace IEEEQualification.Models
{
    public enum UserRole { User, Admin }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public UserRole Role { get; set; } = UserRole.User;

        public List<Review> Reviews { get; set; } = new List<Review>();
        public List<WatchlistItem> Watchlist { get; set; } = new List<WatchlistItem>();
    }
}
