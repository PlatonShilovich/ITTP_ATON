using WebApi_ITTP_ATON.Models;
using WebApi_ITTP_ATON.Repositories;
using WebApi_ITTP_ATON.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> AddUser(AddUserRequestDTO userToAdd, string loginWhoAdds)
    {
        if (userToAdd == null)
            throw new ArgumentNullException(nameof(userToAdd));
        if (string.IsNullOrEmpty(loginWhoAdds))
            throw new ArgumentException("Login of the user who adds cannot be empty", nameof(loginWhoAdds));

        var userWhoAdds = await _userRepository.GetUserByLogin(loginWhoAdds);
        if (userWhoAdds == null)
            throw new InvalidOperationException("User with provided login does not exist");
        if (!userWhoAdds.Admin)
            throw new UnauthorizedAccessException("Only admins can create users");

        return await _userRepository.AddUser(userToAdd, loginWhoAdds);
    }


    public async Task<User> UpdatePersonalData(string loginToUpdate, UpdatePersonalDataRequestDTO personalDataRequest, string loginWhoUpdates)
    {
        if (string.IsNullOrEmpty(loginToUpdate))
            throw new ArgumentException("Login to update cannot be empty", nameof(loginToUpdate));
        if (personalDataRequest == null)
            throw new ArgumentNullException(nameof(personalDataRequest));
        if (string.IsNullOrEmpty(loginWhoUpdates))
            throw new ArgumentException("Login of the user who updates cannot be empty", nameof(loginWhoUpdates));

        var userWhoUpdates = await _userRepository.GetUserByLogin(loginWhoUpdates);
        if (userWhoUpdates == null || userWhoUpdates.RevokedOn != DateTime.MinValue)
            throw new UnauthorizedAccessException("Invalid or revoked user");

        if (!userWhoUpdates.Admin && loginToUpdate != loginWhoUpdates)
            throw new UnauthorizedAccessException("Admins can update all and users can only update themselves");

        var userToUpdate = await _userRepository.GetUserByLogin(loginToUpdate);
        if (userToUpdate == null || userToUpdate.RevokedOn != DateTime.MinValue || userToUpdate.RevokedBy != string.Empty)
            throw new InvalidOperationException("User not found or revoked");

        userToUpdate.Name = personalDataRequest.Name ?? userToUpdate.Name;
        userToUpdate.Gender = personalDataRequest.Gender ?? userToUpdate.Gender;
        userToUpdate.Birthday = personalDataRequest.Birthday ?? userToUpdate.Birthday;
        await _userRepository.UpdateUser(userToUpdate, loginWhoUpdates);
        return userToUpdate;
    }



    public async Task<User> UpdatePassword(string loginToUpdate, UpdatePasswordRequestDTO passwordRequest, string loginWhoUpdates)
    {
        if (string.IsNullOrEmpty(loginToUpdate))
            throw new ArgumentException("Login to update cannot be empty", nameof(loginToUpdate));
        if (passwordRequest == null)
            throw new ArgumentNullException(nameof(passwordRequest));
        if (string.IsNullOrEmpty(loginWhoUpdates))
            throw new ArgumentException("Login of the user who updates cannot be empty", nameof(loginWhoUpdates));

        var userWhoUpdates = await _userRepository.GetUserByLogin(loginWhoUpdates);
        if (userWhoUpdates == null || userWhoUpdates.RevokedOn != DateTime.MinValue)
            throw new UnauthorizedAccessException("Invalid or revoked user");

        if (!userWhoUpdates.Admin && loginToUpdate != loginWhoUpdates)
            throw new UnauthorizedAccessException("Admins can update all and users can only update themselves");

        var userToUpdate = await _userRepository.GetUserByLogin(loginToUpdate);
        if (userToUpdate == null || userToUpdate.RevokedOn != DateTime.MinValue || userToUpdate.RevokedBy != string.Empty)
            throw new InvalidOperationException("User not found or revoked");

        userToUpdate.Password = passwordRequest.Password ?? userToUpdate.Password;
        await _userRepository.UpdateUser(userToUpdate, loginWhoUpdates);
        return userToUpdate;
    }



    public async Task<User> UpdateLogin(string loginToUpdate, UpdateLoginRequestDTO loginRequest, string loginWhoUpdates)
    {
        if (string.IsNullOrEmpty(loginToUpdate))
            throw new ArgumentException("Login to update cannot be empty", nameof(loginToUpdate));
        if (loginRequest == null)
            throw new ArgumentNullException(nameof(loginRequest));
        if (string.IsNullOrEmpty(loginWhoUpdates))
            throw new ArgumentException("Login of the user who updates cannot be empty", nameof(loginWhoUpdates));

        var userWhoUpdates = await _userRepository.GetUserByLogin(loginWhoUpdates);
        if (userWhoUpdates == null || userWhoUpdates.RevokedOn != DateTime.MinValue)
            throw new UnauthorizedAccessException("Invalid or revoked user");

        if (!userWhoUpdates.Admin && loginToUpdate != loginWhoUpdates)
            throw new UnauthorizedAccessException("Admins can update all and users can only update themselves");

        var userToUpdate = await _userRepository.GetUserByLogin(loginToUpdate);
        if (userToUpdate == null || userToUpdate.RevokedOn != DateTime.MinValue || userToUpdate.RevokedBy != string.Empty)
            throw new InvalidOperationException("User not found or revoked");

        if (await _userRepository.GetUserByLogin(loginRequest.Login) != null)
            throw new InvalidOperationException("The provided login is already in use.");

        userToUpdate.Login = loginRequest.Login ?? userToUpdate.Login;
        await _userRepository.UpdateUser(userToUpdate, loginWhoUpdates);
        return userToUpdate;
    }

    public async Task<List<User>> GetActiveUsers(string loginWhoRequests)
    {
        if (string.IsNullOrEmpty(loginWhoRequests))
            throw new ArgumentException("Login of the user who requests cannot be empty", nameof(loginWhoRequests));

        var userWhoRequests = await _userRepository.GetUserByLogin(loginWhoRequests);
        if (userWhoRequests == null || userWhoRequests.RevokedOn != DateTime.MinValue)
            throw new UnauthorizedAccessException("Invalid or revoked user");
        if (!userWhoRequests.Admin)
            throw new UnauthorizedAccessException("Only admins can view active users");

        return await _userRepository.GetActiveUsers(loginWhoRequests);
    }

    public async Task<UserNameGenderBirthdayRevokedOnStatusDTO> GetUserByLogin(string loginToGet, string loginWhoRequests)
    {
        if (string.IsNullOrEmpty(loginToGet))
            throw new ArgumentException("Login to get cannot be empty", nameof(loginToGet));
        if (string.IsNullOrEmpty(loginWhoRequests))
            throw new ArgumentException("Login of the user who requests cannot be empty", nameof(loginWhoRequests));

        var userWhoRequests = await _userRepository.GetUserByLogin(loginWhoRequests);
        if (userWhoRequests == null || userWhoRequests.RevokedOn != DateTime.MinValue)
            throw new UnauthorizedAccessException("Invalid or revoked user");
        if (!userWhoRequests.Admin)
            throw new UnauthorizedAccessException("Only admins can view user details");

        var userToGet = await _userRepository.GetUserByLogin(loginToGet);
        if (userToGet == null)
            throw new InvalidOperationException("User not found");

        return new UserNameGenderBirthdayRevokedOnStatusDTO
        {
            Name = userToGet.Name,
            Gender = userToGet.Gender,
            Birthday = userToGet.Birthday,
            RevokedOn = userToGet.RevokedOn
        };
    }

    public async Task<User> GetUser(GetUserByLoginAndPasswordRequestDTO loginAndPasswordToGet, string whoRequestedLogin)
    {
        if (loginAndPasswordToGet == null)
            throw new ArgumentNullException(nameof(loginAndPasswordToGet));
        if (string.IsNullOrEmpty(whoRequestedLogin))
            throw new ArgumentException("Login of the user who requests cannot be empty", nameof(whoRequestedLogin));

        var userWhoRequests = await _userRepository.GetUserByLogin(whoRequestedLogin);
        if (userWhoRequests == null || userWhoRequests.RevokedOn != DateTime.MinValue)
            throw new UnauthorizedAccessException("Invalid or revoked user");

        if (loginAndPasswordToGet.Login != whoRequestedLogin)
            throw new UnauthorizedAccessException("Users can only access their own data");

        var userToGet = await _userRepository.GetUserByLoginAndPassword(loginAndPasswordToGet.Login, loginAndPasswordToGet.Password);
        if (userToGet == null)
            throw new InvalidOperationException("User not found or invalid credentials");

        return userToGet;
    }

    public async Task<List<User>> GetUsersOverTheAgeOf(int overTheAgeOf, string loginWhoRequests)
    {
        if (overTheAgeOf < 0)
            throw new ArgumentException("Age cannot be negative", nameof(overTheAgeOf));
        if (string.IsNullOrEmpty(loginWhoRequests))
            throw new ArgumentException("Login of the user who requests cannot be empty", nameof(loginWhoRequests));

        var userWhoRequests = await _userRepository.GetUserByLogin(loginWhoRequests);
        if (userWhoRequests == null || userWhoRequests.RevokedOn != DateTime.MinValue)
            throw new UnauthorizedAccessException("Invalid or revoked user");
        if (!userWhoRequests.Admin)
            throw new UnauthorizedAccessException("Only admins can view users by age");

        var thresholdDate = DateTime.UtcNow.AddYears(-overTheAgeOf);
        return await _userRepository.GetUsersOverTheAgeOf(thresholdDate);
    }

    public async Task<User> RevokeUser(string loginToDelete, string loginWhoDeletes)
    {
        if (string.IsNullOrEmpty(loginToDelete))
            throw new ArgumentException("Login to delete cannot be empty", nameof(loginToDelete));
        if (string.IsNullOrEmpty(loginWhoDeletes))
            throw new ArgumentException("Login of the user who deletes cannot be empty", nameof(loginWhoDeletes));

        var userWhoDeletes = await _userRepository.GetUserByLogin(loginWhoDeletes);
        if (userWhoDeletes == null || userWhoDeletes.RevokedOn != DateTime.MinValue)
            throw new UnauthorizedAccessException("Invalid or revoked user");
        if (!userWhoDeletes.Admin)
            throw new UnauthorizedAccessException("Only admins can revoke users");

        var userToDelete = await _userRepository.GetUserByLogin(loginToDelete);
        if (userToDelete == null)
            throw new InvalidOperationException("User not found");

        await _userRepository.RevokeUser(userToDelete, loginWhoDeletes);
        return userToDelete;
    }


    public async Task<User> RestoreUser(string loginToRestore, string loginWhoRestores)
    {
        if (string.IsNullOrEmpty(loginToRestore))
            throw new ArgumentException("Login to restore cannot be empty", nameof(loginToRestore));
        if (string.IsNullOrEmpty(loginWhoRestores))
            throw new ArgumentException("Login of the user who restores cannot be empty", nameof(loginWhoRestores));

        var userWhoRestores = await _userRepository.GetUserByLogin(loginWhoRestores);
        if (userWhoRestores == null || userWhoRestores.RevokedOn != DateTime.MinValue)
            throw new UnauthorizedAccessException("Invalid or revoked user");
        if (!userWhoRestores.Admin)
            throw new UnauthorizedAccessException("Only admins can restore users");

        var userToRestore = await _userRepository.GetUserByLogin(loginToRestore);
        if (userToRestore == null)
            throw new InvalidOperationException("User not found");

        await _userRepository.RestoreUser(userToRestore, loginWhoRestores);
        return userToRestore;
    }
}