using Microsoft.EntityFrameworkCore;
using WebApi_ITTP_ATON.Models;

namespace WebApi_ITTP_ATON.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;

        public UserRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<User> AddUser(AddUserRequestDTO userToAdd, string loginWhoAdds)
        {
            if (userToAdd == null)
                throw new ArgumentNullException(nameof(userToAdd));
            if (string.IsNullOrEmpty(loginWhoAdds))
                throw new ArgumentException("Login of the user who adds cannot be empty", nameof(loginWhoAdds));
            if (string.IsNullOrEmpty(userToAdd.Login))
                throw new ArgumentException("Login cannot be empty", nameof(userToAdd));
            if (string.IsNullOrEmpty(userToAdd.Password))
                throw new ArgumentException("Password cannot be empty", nameof(userToAdd));
            if (string.IsNullOrEmpty(userToAdd.Name))
                throw new ArgumentException("Name cannot be empty", nameof(userToAdd));
            if (await _context.Users.AnyAsync(u => u.Login == userToAdd.Login))
                throw new InvalidOperationException("User already exists");

            var user = new User
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
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserByLogin(string userLogin)
        {
            var userToGet = await _context.Users.FirstOrDefaultAsync(u => u.Login == userLogin);
            if (userToGet == null)
                return null;
            return userToGet;
        }

        public async Task<User?> GetUserByLoginAndPassword(string userLogin, string userPassword)
        {
            var userToGet = await _context.Users.FirstOrDefaultAsync(u => u.Login == userLogin && u.Password == userPassword);
            if (userToGet == null || userToGet.RevokedOn != DateTime.MinValue)
                return null;
            return userToGet;
        }

        public async Task<List<User>> GetActiveUsers(string loginWhoRequests)
        {
            return await _context.Users
                .Where(u => u.RevokedOn == DateTime.MinValue && string.IsNullOrEmpty(u.RevokedBy))
                .OrderByDescending(u => u.CreatedOn)
                .ToListAsync();
        }

        public async Task UpdateUser(User userToUpdate, string loginWhoUpdates)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Guid == userToUpdate.Guid);
            if (existingUser == null)
                throw new InvalidOperationException("User not found");

            existingUser.Login = userToUpdate.Login;
            existingUser.Password = userToUpdate.Password;
            existingUser.Name = userToUpdate.Name;
            existingUser.Gender = userToUpdate.Gender;
            existingUser.Birthday = userToUpdate.Birthday;
            existingUser.ModifiedOn = DateTime.UtcNow;
            existingUser.ModifiedBy = loginWhoUpdates;
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetUsersOverTheAgeOf(DateTime overTheAgeOf)
        {
            return await _context.Users
                .Where(u => u.Birthday != null && u.Birthday < overTheAgeOf)
                .ToListAsync();
        }

        public async Task RevokeUser(User userToRevoke, string loginWhoRevokes)
        {
            var existingUserToRevoke = await _context.Users.FirstOrDefaultAsync(u => u.Guid == userToRevoke.Guid);
            if (existingUserToRevoke == null)
                throw new InvalidOperationException("User not found");

            existingUserToRevoke.RevokedOn = DateTime.UtcNow;
            existingUserToRevoke.RevokedBy = loginWhoRevokes;
            await _context.SaveChangesAsync();
        }
        public async Task RestoreUser(User userToRestore, string loginWhoRestores)
        {
            var existingUserToRestore = await _context.Users.FirstOrDefaultAsync(u => u.Guid == userToRestore.Guid);
            if (existingUserToRestore == null)
                throw new InvalidOperationException("User not found");
            existingUserToRestore.RevokedOn = DateTime.MinValue;
            existingUserToRestore.RevokedBy = string.Empty;
            existingUserToRestore.ModifiedOn = DateTime.UtcNow;
            existingUserToRestore.ModifiedBy = loginWhoRestores;
            await _context.SaveChangesAsync();
        }
    }
}