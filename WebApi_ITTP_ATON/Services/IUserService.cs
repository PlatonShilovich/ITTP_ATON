using WebApi_ITTP_ATON.Models;

namespace WebApi_ITTP_ATON.Services
{
    public interface IUserService
    {
        Task<User> AddUser(AddUserRequestDTO userToAdd, string loginWhoAdds);
        Task<User> UpdatePersonalData(string loginToUpdate, UpdatePersonalDataRequestDTO personalDataRequest, string loginWhoUpdates);
        Task<User> UpdatePassword(string loginToUpdate, UpdatePasswordRequestDTO passwordRequest, string loginWhoUpdates);
        Task<User> UpdateLogin(string loginToUpdate, UpdateLoginRequestDTO loginRequest, string loginWhoUpdates);
        Task<List<User>> GetActiveUsers(string loginWhoRequests);
        Task<UserNameGenderBirthdayRevokedOnStatusDTO> GetUserByLogin(string loginToGet, string loginWhoRequests);
        Task<User> GetUser(GetUserByLoginAndPasswordRequestDTO loginAndPasswordToGet, string whoRequestedLogin);
        Task<List<User>> GetUsersOverTheAgeOf(int overTheAgeOf, string loginWhoRequests);
        Task<User> RevokeUser(string loginToDelete, string loginWhoDeletes);
        Task<User> RestoreUser(string loginToRestore, string loginWhoRestores);
    }
}