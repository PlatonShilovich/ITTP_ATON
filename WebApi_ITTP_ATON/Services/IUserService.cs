using WebApi_ITTP_ATON.Models;

namespace WebApi_ITTP_ATON.Services
{
    public interface IUserService
    {
        Task<User> AddUser(AddUserRequestDTO userToAdd, string loginWhoAdds, CancellationToken cancellationToken);
        Task<User> UpdatePersonalData(string loginToUpdate, UpdatePersonalDataRequestDTO personalDataRequest, string loginWhoUpdates, CancellationToken cancellationToken);
        Task<User> UpdatePassword(string loginToUpdate, UpdatePasswordRequestDTO passwordRequest, string loginWhoUpdates, CancellationToken cancellationToken);
        Task<User> UpdateLogin(string loginToUpdate, UpdateLoginRequestDTO loginRequest, string loginWhoUpdates, CancellationToken cancellationToken);
        Task<List<User>> GetActiveUsers(string loginWhoRequests, CancellationToken cancellationToken);
        Task<UserNameGenderBirthdayRevokedOnStatusDTO> GetUserByLogin(string loginToGet, string loginWhoRequests, CancellationToken cancellationToken);
        Task<User> GetUser(GetUserByLoginAndPasswordRequestDTO loginAndPasswordToGet, string whoRequestedLogin, CancellationToken cancellationToken);
        Task<List<User>> GetUsersOverTheAgeOf(int overTheAgeOf, string loginWhoRequests, CancellationToken cancellationToken);
        Task<User> RevokeUser(string loginToDelete, string loginWhoDeletes, CancellationToken cancellationToken);
        Task<User> RestoreUser(string loginToRestore, string loginWhoRestores, CancellationToken cancellationToken);
    }
}