using Microsoft.AspNetCore.Mvc;
using WebApi_ITTP_ATON.Entities;
using WebApi_ITTP_ATON.Enums;
using WebApi_ITTP_ATON.Models;
using WebApi_ITTP_ATON.Services;

namespace WebApi_ITTP_ATON.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Creates a new user (admin only).
        /// </summary>
        /// <param name="userToAdd">User data to create.</param>
        /// <param name="loginWhoAdds">Login of the admin creating the user.</param>
        /// <returns>Created user data.</returns>
        [HttpPost]
        [RoleFilter(Roles.AdminOnly)]
        public async Task<ActionResult<User>> AddUserRoute([FromBody] AddUserRequestDTO userToAdd, [FromHeader(Name = "ClientInfo")] string loginWhoAdds, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var addedUser = await _userService.AddUser(userToAdd, loginWhoAdds, cancellationToken);
                return CreatedAtAction(nameof(GetUserByLoginRoute), new { loginToGet = userToAdd.Login }, addedUser);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Request was canceled");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest($"Invalid input: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid input: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
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
        [RoleFilter(Roles.AdminOrSelf)]
        public async Task<ActionResult<User>> UpdatePersonalDataRoute(string loginToUpdate, [FromBody] UpdatePersonalDataRequestDTO personalDataRequest, [FromHeader(Name = "ClientInfo")] string loginWhoUpdates, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userToUpdate = await _userService.UpdatePersonalData(loginToUpdate, personalDataRequest, loginWhoUpdates, cancellationToken);
                return Ok(userToUpdate);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Request was canceled");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest($"Invalid input: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid input: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the password of a user (admin or self).
        /// </summary>
        /// <param name="loginToUpdate">Login of the user to update.</param>
        /// <param name="passwordRequest">New password.</param>
        /// <param name="loginWhoUpdates">Login of the user performing the update.</param>
        /// <returns>Updated user data.</returns>
        [HttpPut("users/{loginToUpdate}/password")]
        [RoleFilter(Roles.AdminOrSelf)]
        public async Task<ActionResult<User>> UpdatePasswordRoute(string loginToUpdate, [FromBody] UpdatePasswordRequestDTO passwordRequest, [FromHeader(Name = "ClientInfo")] string loginWhoUpdates, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userToUpdate = await _userService.UpdatePassword(loginToUpdate, passwordRequest, loginWhoUpdates, cancellationToken);
                return Ok(userToUpdate);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Request was canceled");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest($"Invalid input: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid input: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the login of a user (admin or self).
        /// </summary>
        /// <param name="loginToUpdate">Current login of the user to update.</param>
        /// <param name="loginRequest">New login.</param>
        /// <param name="loginWhoUpdates">Login of the user performing the update.</param>
        /// <returns>Updated user data.</returns>
        [HttpPut("users/{loginToUpdate}/login")]
        [RoleFilter(Roles.AdminOrSelf)]
        public async Task<ActionResult<User>> UpdateLoginRoute(string loginToUpdate, [FromBody] UpdateLoginRequestDTO loginRequest, [FromHeader(Name = "ClientInfo")] string loginWhoUpdates, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userToUpdate = await _userService.UpdateLogin(loginToUpdate, loginRequest, loginWhoUpdates, cancellationToken);
                return Ok(userToUpdate);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Request was canceled");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest($"Invalid input: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid input: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the list of active users (admin only).
        /// </summary>
        /// <param name="loginWhoRequests">Login of the admin requesting the data.</param>
        /// <returns>List of active users.</returns>
        [HttpGet("active-users")]
        [RoleFilter(Roles.AdminOnly)]
        public async Task<ActionResult<List<User>>> GetActiveUsersRoute([FromHeader(Name = "ClientInfo")] string loginWhoRequests, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var users = await _userService.GetActiveUsers(loginWhoRequests, cancellationToken);
                return Ok(users);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Request was canceled");
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid input: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets user data by login (admin only).
        /// </summary>
        /// <param name="loginToGet">Login of the user to retrieve.</param>
        /// <param name="loginWhoRequests">Login of the user requesting the data.</param>
        /// <returns>User data (name, gender, birthday, status).</returns>
        [HttpGet("user/{loginToGet}")]
        [RoleFilter(Roles.AdminOnly)]
        public async Task<ActionResult<UserNameGenderBirthdayRevokedOnStatusDTO>> GetUserByLoginRoute(string loginToGet, [FromHeader(Name = "ClientInfo")] string loginWhoRequests, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _userService.GetUserByLogin(loginToGet, loginWhoRequests, cancellationToken);
                return Ok(user);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Request was canceled");
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid input: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets user data by login and password (self only).
        /// </summary>
        /// <param name="loginAndPasswordToGet">Login and password of the user to retrieve (via query parameters).</param>
        /// <param name="whoRequestedLogin">Login of the user requesting the data.</param>
        /// <returns>User data.</returns>
        [HttpGet("user")]
        [RoleFilter(Roles.SelfOnly)]
        public async Task<ActionResult<User>> GetUserRoute([FromQuery] GetUserByLoginAndPasswordRequestDTO loginAndPasswordToGet, [FromHeader(Name = "ClientInfo")] string whoRequestedLogin, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _userService.GetUser(loginAndPasswordToGet, whoRequestedLogin, cancellationToken);
                return Ok(user);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Request was canceled");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest($"Invalid input: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid input: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets users older than a specified age (admin only).
        /// </summary>
        /// <param name="overTheAgeOf">Age threshold.</param>
        /// <param name="loginWhoRequests">Login of the admin requesting the data.</param>
        /// <returns>List of users older than the specified age.</returns>
        [HttpGet("users/older-than/{overTheAgeOf}")]
        [RoleFilter(Roles.AdminOnly)]
        public async Task<ActionResult<List<User>>> GetUsersOverTheAgeOfRoute(int overTheAgeOf, [FromHeader(Name = "ClientInfo")] string loginWhoRequests, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var users = await _userService.GetUsersOverTheAgeOf(overTheAgeOf, loginWhoRequests, cancellationToken);
                return Ok(users);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Request was canceled");
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid input: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Revokes a user (admin only).
        /// </summary>
        /// <param name="loginToDelete">Login of the user to revoke.</param>
        /// <param name="loginWhoDeletes">Login of the admin performing the revocation.</param>
        /// <returns>Revoked user data.</returns>
        [HttpDelete("users/{loginToDelete}")]
        [RoleFilter(Roles.AdminOnly)]
        public async Task<ActionResult<User>> RevokeUserRoute(string loginToDelete, [FromHeader(Name = "ClientInfo")] string loginWhoDeletes, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _userService.RevokeUser(loginToDelete, loginWhoDeletes, cancellationToken);
                return Ok(user);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Request was canceled");
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid input: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Restores a revoked user (admin only).
        /// </summary>
        /// <param name="loginToRestore">Login of the user to restore.</param>
        /// <param name="loginWhoRestores">Login of the admin performing the restoration.</param>
        /// <returns>Restored user data.</returns>
        [HttpPut("users/{loginToRestore}/restore")]
        [RoleFilter(Roles.AdminOnly)]
        public async Task<ActionResult<User>> RestoreUserRoute(string loginToRestore, [FromHeader(Name = "ClientInfo")] string loginWhoRestores, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _userService.RestoreUser(loginToRestore, loginWhoRestores, cancellationToken);
                return Ok(user);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Request was canceled");
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid input: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}