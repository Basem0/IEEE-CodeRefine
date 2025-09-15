using System.Linq;
using IEEEQualification.Data;
using IEEEQualification.Models;

namespace IEEEQualification.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public User? Login(string username, string password)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        public User Register(string username, string email, string password, UserRole role = UserRole.User)
        {
            var exists = _context.Users.Any(u => u.Username == username || u.Email == email);
            if (exists) throw new Exception("Username or Email already exists.");

            var user = new User { Username = username, Email = email, Password = password, Role = role };
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public void SeedAdmin()
        {
            if (!_context.Users.Any(u => u.Role == UserRole.Admin))
            {
                Register("admin", "admin@ieeemdb.com", "admin123", UserRole.Admin);
            }
        }

    }
}
