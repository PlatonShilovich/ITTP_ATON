using WebApi_ITTP_ATON.Models;

namespace WebApi_ITTP_ATON.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = new List<User>();

        public InMemoryUserRepository()
        {
            _users.Add(new User
            {
                Guid = Guid.NewGuid(),
                Login = "Admin",
                Password = "admin",
                Name = "Admin",
                Gender = 2,
                Birthday = null,
                Admin = true,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System",
                ModifiedOn = DateTime.UtcNow,
                ModifiedBy = "System",
                RevokedOn = DateTime.MinValue,
                RevokedBy = string.Empty
            });
        }

        public void AddUser(User user){
            if (_users.Any(u => u.Login == user.Login))
            {
                throw new InvalidOperationException("User already exists");
            }
            _users.Add(user);
        }

        public User? GetUserByLogin(string login){
            return _users.FirstOrDefault(u => u.Login == login);
        }

        public List<User> GetActiveUsers(){
            return _users.Where(u => u.RevokedOn == DateTime.MinValue).ToList();
        }
    }
}