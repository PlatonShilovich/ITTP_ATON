using WebApi_ITTP_ATON.Models;

namespace WebApi_ITTP_ATON.Services
{
    public interface IUserService
    {
        Task<UserDto> AddUser(AddUserDto userToAdd, string loginWhoAdds, CancellationToken cancellationToken = default);
        Task<UserDto> UpdatePersonalData(string loginToUpdate, UpdatePersonalDataDto personalDataRequest, string loginWhoUpdates, CancellationToken cancellationToken = default);
        Task<UserDto> UpdatePassword(string loginToUpdate, UpdatePasswordDto passwordRequest, string loginWhoUpdates, CancellationToken cancellationToken = default);
        Task<UserDto> UpdateLogin(string loginToUpdate, UpdateLoginDto loginRequest, string loginWhoUpdates, CancellationToken cancellationToken = default);
        Task<List<UserDto>> GetActiveUsers(string loginWhoRequests, CancellationToken cancellationToken = default);
        Task<GetPersonalDataDto> GetUserByLogin(string loginToGet, string loginWhoRequests, CancellationToken cancellationToken = default);
        Task<UserDto> GetUser(GetUserByLoginAndPasswordDto loginAndPasswordToGet, string whoRequestedLogin, CancellationToken cancellationToken = default);
        Task<List<UserDto>> GetUsersOverTheAgeOf(int overTheAgeOf, string loginWhoRequests, CancellationToken cancellationToken = default);
        Task<UserDto> RevokeUser(string loginToDelete, string loginWhoDeletes, CancellationToken cancellationToken = default);
        Task<UserDto> RestoreUser(string loginToRestore, string loginWhoRestores, CancellationToken cancellationToken = default);
    }
}