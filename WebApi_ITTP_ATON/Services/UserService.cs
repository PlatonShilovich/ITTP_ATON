using WebApi_ITTP_ATON.Entities;
using WebApi_ITTP_ATON.Models;
using WebApi_ITTP_ATON.Repositories;

namespace WebApi_ITTP_ATON.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> AddUser(AddUserDto userToAdd, string loginWhoAdds, CancellationToken cancellationToken = default)
        {
            var existingUser = await _userRepository.GetUserByLogin(userToAdd.Login, cancellationToken);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Пользователь с таким логином уже существует.");
            }

            var user = await _userRepository.AddUser(userToAdd, loginWhoAdds, cancellationToken);
            return MapToDto(user);
        }

        public async Task<UserDto> UpdatePersonalData(string loginToUpdate, UpdatePersonalDataDto personalDataRequest, string loginWhoUpdates, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetUserByLogin(loginToUpdate, cancellationToken);
            if (user == null || user.RevokedOn != null)
                throw new InvalidOperationException("Пользователь не найден или отозван");

            user.Name = personalDataRequest.Name ?? user.Name;
            user.Gender = personalDataRequest.Gender ?? user.Gender;
            user.Birthday = personalDataRequest.Birthday ?? user.Birthday;
            await _userRepository.UpdateUser(user, loginWhoUpdates, cancellationToken);
            return MapToDto(user);
        }

        public async Task<UserDto> UpdatePassword(string loginToUpdate, UpdatePasswordDto passwordRequest, string loginWhoUpdates, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetUserByLogin(loginToUpdate, cancellationToken);
            if (user == null || user.RevokedOn != null)
                throw new InvalidOperationException("Пользователь не найден или отозван");

            user.Password = passwordRequest.Password ?? user.Password;
            await _userRepository.UpdateUser(user, loginWhoUpdates, cancellationToken);
            return MapToDto(user);
        }

        public async Task<UserDto> UpdateLogin(string loginToUpdate, UpdateLoginDto loginRequest, string loginWhoUpdates, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetUserByLogin(loginToUpdate, cancellationToken);
            if (user == null || user.RevokedOn != null)
                throw new InvalidOperationException("Пользователь не найден или отозван");

            user.Login = loginRequest.Login ?? user.Login;
            await _userRepository.UpdateUser(user, loginWhoUpdates, cancellationToken);
            return MapToDto(user);
        }

        public async Task<List<UserDto>> GetActiveUsers(string loginWhoRequests, CancellationToken cancellationToken = default)
        {
            var users = await _userRepository.GetActiveUsers(cancellationToken);
            return users.Select(MapToDto).ToList();
        }

        public async Task<GetPersonalDataDto> GetUserByLogin(string loginToGet, string loginWhoRequests, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetUserByLogin(loginToGet, cancellationToken);
            if (user == null)
                throw new InvalidOperationException("Пользователь не найден");

            return new GetPersonalDataDto
            {
                Name = user.Name,
                Gender = user.Gender,
                Birthday = user.Birthday,
                RevokedOn = user.RevokedOn ?? DateTime.MinValue
            };
        }

        public async Task<UserDto> GetUser(GetUserByLoginAndPasswordDto loginAndPasswordToGet, string whoRequestedLogin, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetUserByLoginAndPassword(loginAndPasswordToGet.Login, loginAndPasswordToGet.Password, cancellationToken);
            if (user == null || user.RevokedOn != null)
                throw new InvalidOperationException("Пользователь не найден или отозван");

            return MapToDto(user);
        }

        public async Task<List<UserDto>> GetUsersOverTheAgeOf(int overTheAgeOf, string loginWhoRequests, CancellationToken cancellationToken = default)
        {
            var thresholdDate = DateTime.UtcNow.AddYears(-overTheAgeOf);
            var users = await _userRepository.GetUsersOverTheAgeOf(thresholdDate, cancellationToken);
            return users.Select(MapToDto).ToList();
        }

        public async Task<UserDto> RevokeUser(string loginToDelete, string loginWhoDeletes, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetUserByLogin(loginToDelete, cancellationToken);
            if (user == null)
                throw new InvalidOperationException("Пользователь не найден");

            await _userRepository.RevokeUser(user, loginWhoDeletes, cancellationToken);
            return MapToDto(user);
        }

        public async Task<UserDto> RestoreUser(string loginToRestore, string loginWhoRestores, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetUserByLogin(loginToRestore, cancellationToken);
            if (user == null)
                throw new InvalidOperationException("Пользователь не найден");

            await _userRepository.RestoreUser(user, loginWhoRestores, cancellationToken);
            return MapToDto(user);
        }

        private UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Guid = user.Guid,
                Login = user.Login,
                Password = user.Password,
                Name = user.Name,
                Gender = user.Gender,
                Birthday = user.Birthday,
                Admin = user.Admin,
                CreatedOn = user.CreatedOn,
                CreatedBy = user.CreatedBy,
                ModifiedOn = user.ModifiedOn,
                ModifiedBy = user.ModifiedBy,
                RevokedOn = user.RevokedOn,
                RevokedBy = user.RevokedBy
            };
        }
    }
}