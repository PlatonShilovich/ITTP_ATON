using WebApi_ITTP_ATON.Models;

namespace WebApi_ITTP_ATON.Repositories
{
    public interface IUserRepository
    {
        void AddUser(AddUserRequestDTO userToAdd, string loginWhoAdds);
        User? GetUserByLogin(string userLogin);
        User? GetUserByLoginAndPassword(string userLogin, string userPassword);
        List<User>? GetActiveUsers(string loginWhoRequests);
        void UpdateUser(User userToUpdate, string loginWhoUpdates);
        List<User>? GetUsersOverTheAgeOf(DateTime date);
        void RevokeUser(User userToRevoke, string loginWhoRevokes);
        void RestoreUser(User userToRestore, string loginWhoRestores);
    }
}