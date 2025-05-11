using Microsoft.AspNetCore.Mvc;
using WebApi_ITTP_ATON.Repositories;
using WebApi_ITTP_ATON.Models;

namespace WebApi_ITTP_ATON.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Creates a new user (admin only).
        /// </summary>
        /// <param name="userToAdd">User data to create.</param>
        /// <param name="loginWhoAdds">Login of the admin creating the user.</param>
        /// <returns>Created user data.</returns>
        [HttpPost]
        public ActionResult<User> AddUserRoute([FromBody] AddUserRequestDTO userToAdd, [FromHeader(Name = "ClientInfo")] string loginWhoAdds)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userWhoAdds = _userRepository.GetUserByLogin(loginWhoAdds);
            if (userWhoAdds == null || !userWhoAdds.Admin)
                return Unauthorized("Only admins can create users");

            try
            {
                _userRepository.AddUser(userToAdd, userWhoAdds.Login);
                var addedUser = _userRepository.GetUserByLogin(userToAdd.Login);
                if (addedUser == null)
                    return StatusCode(500, "Failed to retrieve created user");

                return CreatedAtAction(nameof(GetUserByLoginRoute), new { loginToGet = userToAdd.Login }, addedUser);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates personal data (name, gender, birthday) of a user (admin or self).
        /// </summary>
        /// <param name="loginToUpdate">Login of the user to update.</param>
        /// <param name="personalDataRequest">Updated personal data.</param>
        /// <param name="loginWhoUpdates">Login of the user performing the update.</param>
        /// <returns>Updated user data.</returns>
        [HttpPut("users/{loginToUpdate}/personal-data")]
        public ActionResult<User> UpdatePersonalDataRoute(string loginToUpdate, [FromBody] UpdatePersonalDataRequestDTO personalDataRequest, [FromHeader(Name = "ClientInfo")] string loginWhoUpdates)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userWhoUpdates = _userRepository.GetUserByLogin(loginWhoUpdates);
            if (userWhoUpdates == null || userWhoUpdates.RevokedOn != DateTime.MinValue)
                return Unauthorized("Invalid or revoked user");

            if (!userWhoUpdates.Admin && loginToUpdate != loginWhoUpdates)
                return Unauthorized("Admins can update all and users can only update themselves");

            var userToUpdate = _userRepository.GetUserByLogin(loginToUpdate);
            if (userToUpdate == null || userToUpdate.RevokedOn != DateTime.MinValue || userToUpdate.RevokedBy != string.Empty)
                return NotFound("User not found or revoked");

            userToUpdate.Name = personalDataRequest.Name ?? userToUpdate.Name;
            userToUpdate.Gender = personalDataRequest.Gender ?? userToUpdate.Gender;
            userToUpdate.Birthday = personalDataRequest.Birthday ?? userToUpdate.Birthday;
            _userRepository.UpdateUser(userToUpdate, loginWhoUpdates);
            return Ok(userToUpdate);
        }

        /// <summary>
        /// Updates the password of a user (admin or self).
        /// </summary>
        /// <param name="loginToUpdate">Login of the user to update.</param>
        /// <param name="passwordRequest">New password.</param>
        /// <param name="loginWhoUpdates">Login of the user performing the update.</param>
        /// <returns>Updated user data.</returns>
        [HttpPut("users/{loginToUpdate}/password")]
        public ActionResult<User> UpdatePasswordRoute(string loginToUpdate, [FromBody] UpdatePasswordRequestDTO passwordRequest, [FromHeader(Name = "ClientInfo")] string loginWhoUpdates)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userWhoUpdates = _userRepository.GetUserByLogin(loginWhoUpdates);
            if (userWhoUpdates == null || userWhoUpdates.RevokedOn != DateTime.MinValue)
                return Unauthorized("Invalid or revoked user");

            if (!userWhoUpdates.Admin && loginToUpdate != loginWhoUpdates)
                return Unauthorized("Admins can update all and users can only update themselves");

            var userToUpdate = _userRepository.GetUserByLogin(loginToUpdate);
            if (userToUpdate == null || userToUpdate.RevokedOn != DateTime.MinValue || userToUpdate.RevokedBy != string.Empty)
                return NotFound("User not found or revoked");

            userToUpdate.Password = passwordRequest.Password ?? userToUpdate.Password;
            _userRepository.UpdateUser(userToUpdate, loginWhoUpdates);
            return Ok(userToUpdate);
        }

        /// <summary>
        /// Updates the login of a user (admin or self).
        /// </summary>
        /// <param name="loginToUpdate">Current login of the user to update.</param>
        /// <param name="loginRequest">New login.</param>
        /// <param name="loginWhoUpdates">Login of the user performing the update.</param>
        /// <returns>Updated user data.</returns>
        [HttpPut("users/{loginToUpdate}/login")]
        public ActionResult<User> UpdateLoginRoute(string loginToUpdate, [FromBody] UpdateLoginRequestDTO loginRequest, [FromHeader(Name = "ClientInfo")] string loginWhoUpdates)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userWhoUpdates = _userRepository.GetUserByLogin(loginWhoUpdates);
            if (userWhoUpdates == null || userWhoUpdates.RevokedOn != DateTime.MinValue)
                return Unauthorized("Invalid or revoked user");

            if (!userWhoUpdates.Admin && loginToUpdate != loginWhoUpdates)
                return Unauthorized("Admins can update all and users can only update themselves");

            var userToUpdate = _userRepository.GetUserByLogin(loginToUpdate);
            if (userToUpdate == null || userToUpdate.RevokedOn != DateTime.MinValue || userToUpdate.RevokedBy != string.Empty)
                return NotFound("User not found or revoked");

            if (_userRepository.GetUserByLogin(loginRequest.Login) != null)
                return BadRequest("The provided login is already in use.");

            userToUpdate.Login = loginRequest.Login ?? userToUpdate.Login;
            _userRepository.UpdateUser(userToUpdate, loginWhoUpdates);
            return Ok(userToUpdate);
        }

        /// <summary>
        /// Gets the list of active users (admin only).
        /// </summary>
        /// <param name="loginWhoRequests">Login of the admin requesting the data.</param>
        /// <returns>List of active users.</returns>
        [HttpGet("active-users")]
        public ActionResult<List<User>> GetActiveUsersRoute([FromHeader(Name = "ClientInfo")] string loginWhoRequests)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userWhoRequests = _userRepository.GetUserByLogin(loginWhoRequests);
            if (userWhoRequests == null || !userWhoRequests.Admin)
                return Unauthorized("Invalid or not admin");

            return Ok(_userRepository.GetActiveUsers(loginWhoRequests));
        }

        /// <summary>
        /// Gets user data by login (admin only).
        /// </summary>
        /// <param name="loginToGet">Login of the user to retrieve.</param>
        /// <param name="loginWhoRequests">Login of the user requesting the data.</param>
        /// <returns>User data (name, gender, birthday, status).</returns>
        [HttpGet("user/{loginToGet}")]
        public ActionResult<UserNameGenderBirthdayRevokedOnStatusDTO> GetUserByLoginRoute(string loginToGet, [FromHeader(Name = "ClientInfo")] string loginWhoRequests)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userWhoRequests = _userRepository.GetUserByLogin(loginWhoRequests);
            if (userWhoRequests == null || !userWhoRequests.Admin)
                return Unauthorized("Invalid or not admin");

            var userToGet = _userRepository.GetUserByLogin(loginToGet);
            if (userToGet == null)
                return NotFound("User not found");

            var response = new UserNameGenderBirthdayRevokedOnStatusDTO
            {
                Name = userToGet.Name,
                Gender = userToGet.Gender,
                Birthday = userToGet.Birthday,
                RevokedOn = userToGet.RevokedOn
            };
            return Ok(response);
        }

        /// <summary>
        /// Gets user data by login and password (self only).
        /// </summary>
        /// <param name="loginAndPasswordToGet">Login and password of the user to retrieve (via query parameters).</param>
        /// <param name="whoRequestedLogin">Login of the user requesting the data.</param>
        /// <returns>User data.</returns>
        [HttpGet("user")]
        public ActionResult<User> GetUserRoute([FromQuery] GetUserByLoginAndPasswordRequestDTO loginAndPasswordToGet, [FromHeader(Name = "ClientInfo")] string whoRequestedLogin)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userWhoRequests = _userRepository.GetUserByLogin(whoRequestedLogin);
            if (userWhoRequests == null || userWhoRequests.RevokedOn != DateTime.MinValue || userWhoRequests.RevokedBy != string.Empty)
                return Unauthorized("Invalid or revoked user");

            if (loginAndPasswordToGet.Login != whoRequestedLogin)
                return Unauthorized("Users can only access their own data");

            var user = _userRepository.GetUserByLoginAndPassword(loginAndPasswordToGet.Login, loginAndPasswordToGet.Password);
            if (user == null)
                return NotFound("User not found or invalid credentials");

            return Ok(user);
        }
        [HttpGet("users/older-than/{age}")]
        public ActionResult<List<User>> GetUsersOverTheAgeOfRoute(int overTheAgeOf, [FromHeader(Name = "ClientInfo")] string loginWhoRequests)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userWhoRequests = _userRepository.GetUserByLogin(loginWhoRequests);
            if (userWhoRequests == null || !userWhoRequests.Admin)
                return Unauthorized("Invalid or not admin");
            var thresholdDate = DateTime.UtcNow.AddYears(-overTheAgeOf);
            return Ok(_userRepository.GetUsersOverTheAgeOf(thresholdDate));
        }

        [HttpDelete("users/{loginToDelete}")]
        public ActionResult<User> RevokeUserRoute(string loginToDelete, [FromHeader(Name = "ClientInfo")] string loginWhoDeletes)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userWhoDeletes = _userRepository.GetUserByLogin(loginWhoDeletes);
            if (userWhoDeletes == null || !userWhoDeletes.Admin)
                return Unauthorized("Invalid or not admin");

            var userToDelete = _userRepository.GetUserByLogin(loginToDelete);
            if (userToDelete == null)
                return NotFound("User not found");

            _userRepository.RevokeUser(userToDelete, loginWhoDeletes);
            return Ok(userToDelete);
        }

        [HttpPut("users/{loginToRestore}/restore")]
        public ActionResult<User> RestoreUserRoute(string loginToRestore, [FromHeader(Name = "ClientInfo")] string loginWhoRestores)
        {
            var userWhoRestores = _userRepository.GetUserByLogin(loginWhoRestores);
            if (userWhoRestores == null || !userWhoRestores.Admin)
                return Unauthorized("Invalid or not admin");
            var userToRestore = _userRepository.GetUserByLogin(loginToRestore);
            if (userToRestore == null)
                return NotFound("User not found");
            _userRepository.RestoreUser(userToRestore, loginWhoRestores);
            return Ok(userToRestore);
        }
    }
}