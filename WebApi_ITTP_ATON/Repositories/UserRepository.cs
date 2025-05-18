using Microsoft.EntityFrameworkCore;
using WebApi_ITTP_ATON.Entities;
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

        public async Task<User> AddUser(AddUserDto userToAdd, string loginWhoAdds, CancellationToken cancellationToken = default)
        {
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
            };
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return user;
        }

        public async Task<User?> GetUserByLogin(string userLogin, CancellationToken cancellationToken = default)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Login == userLogin, cancellationToken);
        }

        public async Task<User?> GetUserByLoginAndPassword(string userLogin, string userPassword, CancellationToken cancellationToken = default)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Login == userLogin && u.Password == userPassword, cancellationToken);
        }

        public async Task<List<User>> GetActiveUsers(CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .Where(u => u.RevokedOn == null)
                .OrderByDescending(u => u.CreatedOn)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateUser(User userToUpdate, string loginWhoUpdates, CancellationToken cancellationToken = default)
        {
            userToUpdate.ModifiedOn = DateTime.UtcNow;
            userToUpdate.ModifiedBy = loginWhoUpdates;
            _context.Users.Update(userToUpdate);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<User>> GetUsersOverTheAgeOf(DateTime overTheAgeOf, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .Where(u => u.Birthday != null && u.Birthday < overTheAgeOf)
                .ToListAsync(cancellationToken);
        }

        public async Task RevokeUser(User userToRevoke, string loginWhoRevokes, CancellationToken cancellationToken = default)
        {
            userToRevoke.RevokedOn = DateTime.UtcNow;
            userToRevoke.RevokedBy = loginWhoRevokes;
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RestoreUser(User userToRestore, string loginWhoRestores, CancellationToken cancellationToken = default)
        {
            userToRestore.RevokedOn = null;
            userToRestore.RevokedBy = null;
            userToRestore.ModifiedOn = DateTime.UtcNow;
            userToRestore.ModifiedBy = loginWhoRestores;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}