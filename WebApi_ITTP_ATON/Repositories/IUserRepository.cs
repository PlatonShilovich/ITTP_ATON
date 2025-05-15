using WebApi_ITTP_ATON.Models;

namespace WebApi_ITTP_ATON.Repositories
{
    public interface IUserRepository
    {
        Task<User> AddUser(AddUserRequestDTO userToAdd, string loginWhoAdds);
        Task<User?> GetUserByLogin(string userLogin);
        Task<User?> GetUserByLoginAndPassword(string userLogin, string userPassword);
        Task<List<User>> GetActiveUsers(string loginWhoRequests);
        Task UpdateUser(User userToUpdate, string loginWhoUpdates);
        Task<List<User>> GetUsersOverTheAgeOf(DateTime date);
        Task RevokeUser(User userToRevoke, string loginWhoRevokes);
        Task RestoreUser(User userToRestore, string loginWhoRestores);
    }
}