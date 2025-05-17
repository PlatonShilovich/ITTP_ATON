using WebApi_ITTP_ATON.Entities;
using WebApi_ITTP_ATON.Models;

namespace WebApi_ITTP_ATON.Repositories
{
    public interface IUserRepository
    {
        Task<User> AddUser(AddUserRequestDTO userToAdd, string loginWhoAdds, CancellationToken cancellationToken);
        Task<User?> GetUserByLogin(string userLogin, CancellationToken cancellationToken);
        Task<User?> GetUserByLoginAndPassword(string userLogin, string userPassword, CancellationToken cancellationToken);
        Task<List<User>> GetActiveUsers(string loginWhoRequests, CancellationToken cancellationToken);
        Task UpdateUser(User userToUpdate, string loginWhoUpdates, CancellationToken cancellationToken);
        Task<List<User>> GetUsersOverTheAgeOf(DateTime date, CancellationToken cancellationToken);
        Task RevokeUser(User userToRevoke, string loginWhoRevokes, CancellationToken cancellationToken);
        Task RestoreUser(User userToRestore, string loginWhoRestores, CancellationToken cancellationToken);
    }
}