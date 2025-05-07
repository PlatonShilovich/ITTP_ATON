using WebApi_ITTP_ATON.Models;

namespace WebApi_ITTP_ATON.Repositories
{
    public interface IUserRepository
    {
        void AddUser(User user);
        User? GetUserByLogin(string login);
        List<User> GetActiveUsers();
    }
}