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

        public void AddUser(AddUserRequestDTO userToAdd, string loginWhoAdds)
        {
            if (userToAdd == null)
                throw new ArgumentNullException(nameof(userToAdd));
            if (_users.Any(u => u.Login == userToAdd.Login))
                throw new InvalidOperationException("User already exists");

            _users.Add(new User
            {
                Guid = Guid.NewGuid(),
                Login = userToAdd.Login,
                Password = userToAdd.Password,
                Name = userToAdd.Name,
                Gender = userToAdd.Gender,
                Birthday = userToAdd.Birthday,
                Admin = userToAdd.Admin,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = loginWhoAdds,
                ModifiedOn = DateTime.UtcNow,
                ModifiedBy = loginWhoAdds,
                RevokedOn = DateTime.MinValue,
                RevokedBy = string.Empty
            });
        }

        public User? GetUserByLogin(string userLogin)
        {
            var userToGet = _users.FirstOrDefault(u => u.Login == userLogin);
            if (userToGet == null)
                return null;
            return userToGet;
        }

        public User? GetUserByLoginAndPassword(string userLogin, string userPassword)
        {
            var userToGet = _users.FirstOrDefault(u => u.Login == userLogin && u.Password == userPassword);
            if (userToGet == null || userToGet.RevokedOn != DateTime.MinValue)
                return null;
            return userToGet;
        }

        public List<User>? GetActiveUsers(string loginWhoRequests)
        {
            return _users
                .Where(u => u.RevokedOn == DateTime.MinValue && string.IsNullOrEmpty(u.RevokedBy))
                .OrderByDescending(u => u.CreatedOn)
                .ToList();
        }

        public void UpdateUser(User userToUpdate, string loginWhoUpdates)
        {
            var existingUser = _users.FirstOrDefault(u => u.Guid == userToUpdate.Guid);
            if (existingUser == null)
                throw new InvalidOperationException("User not found");

            existingUser.Login = userToUpdate.Login;
            existingUser.Password = userToUpdate.Password;
            existingUser.Name = userToUpdate.Name;
            existingUser.Gender = userToUpdate.Gender;
            existingUser.Birthday = userToUpdate.Birthday;
            existingUser.ModifiedOn = DateTime.UtcNow;
            existingUser.ModifiedBy = loginWhoUpdates;
        }

        public List<User>? GetUsersOverTheAgeOf(DateTime overTheAgeOf)
        {
            return _users
                .Where(u => u.Birthday != null && u.Birthday < overTheAgeOf)
                .ToList();
        }

        public void RevokeUser(User userToRevoke, string loginWhoRevokes)
        {
            var existingUserToRevoke = _users.FirstOrDefault(u => u.Guid == userToRevoke.Guid);
            if (existingUserToRevoke == null)
                throw new InvalidOperationException("User not found");

            existingUserToRevoke.RevokedOn = DateTime.UtcNow;
            existingUserToRevoke.RevokedBy = loginWhoRevokes;
        }
        public void RestoreUser(User userToRestore, string loginWhoRestores)
        {
            var existingUserToRestore = _users.FirstOrDefault(u => u.Guid == userToRestore.Guid);
            if (existingUserToRestore == null)
                throw new InvalidOperationException("User not found");
            existingUserToRestore.RevokedOn = DateTime.MinValue;
            existingUserToRestore.RevokedBy = string.Empty;
            existingUserToRestore.ModifiedOn = DateTime.UtcNow;
            existingUserToRestore.ModifiedBy = loginWhoRestores;
        }
    }
}