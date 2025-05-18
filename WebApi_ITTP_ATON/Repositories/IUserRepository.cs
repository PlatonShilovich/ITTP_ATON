using WebApi_ITTP_ATON.Entities;
using WebApi_ITTP_ATON.Models;

namespace WebApi_ITTP_ATON.Repositories
{
    public interface IUserRepository
    {
        Task<User> AddUser(AddUserDto userToAdd, string loginWhoAdds, CancellationToken cancellationToken = default);
        Task<User?> GetUserByLogin(string userLogin, CancellationToken cancellationToken = default);
        Task<User?> GetUserByLoginAndPassword(string userLogin, string userPassword, CancellationToken cancellationToken = default);
        Task<List<User>> GetActiveUsers(CancellationToken cancellationToken = default);
        Task UpdateUser(User userToUpdate, string loginWhoUpdates, CancellationToken cancellationToken = default);
        Task<List<User>> GetUsersOverTheAgeOf(DateTime date, CancellationToken cancellationToken = default);
        Task RevokeUser(User userToRevoke, string loginWhoRevokes, CancellationToken cancellationToken = default);
        Task RestoreUser(User userToRestore, string loginWhoRestores, CancellationToken cancellationToken = default);
    }
}