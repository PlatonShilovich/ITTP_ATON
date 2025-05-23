using Microsoft.AspNetCore.Mvc;
using WebApi_ITTP_ATON.Enums;
using WebApi_ITTP_ATON.Filters;
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

        [HttpPost()]
        [RoleFilter(Roles.AdminOnly)]
        public async Task<ActionResult<Guid>> AddUser([FromBody] AddUserDto userToAdd, [FromHeader(Name = "ClientInfo")] string loginWhoAdds, CancellationToken cancellationToken = default)
        {       
            try
            {
                var addedUser = await _userService.AddUser(userToAdd, loginWhoAdds, cancellationToken);
                return Ok(addedUser.Guid);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("personal-data/{loginToUpdate}")]
        [RoleFilter(Roles.AdminOrSelf)]
        public async Task<ActionResult<UserDto>> UpdatePersonalData(string loginToUpdate, [FromBody] UpdatePersonalDataDto personalDataRequest, [FromHeader(Name = "ClientInfo")] string loginWhoUpdates, CancellationToken cancellationToken = default)
        {
            try
            {
                var userToUpdate = await _userService.UpdatePersonalData(loginToUpdate, personalDataRequest, loginWhoUpdates, cancellationToken);
                return Ok(userToUpdate);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("password/{loginToUpdate}")]
        [RoleFilter(Roles.AdminOrSelf)]
        public async Task<ActionResult<UserDto>> UpdatePassword(string loginToUpdate, [FromBody] UpdatePasswordDto passwordRequest, [FromHeader(Name = "ClientInfo")] string loginWhoUpdates, CancellationToken cancellationToken = default)
        {
            try
            {
                var userToUpdate = await _userService.UpdatePassword(loginToUpdate, passwordRequest, loginWhoUpdates, cancellationToken);
                return Ok(userToUpdate);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("login/{loginToUpdate}")]
        [RoleFilter(Roles.AdminOrSelf)]
        public async Task<ActionResult<UserDto>> UpdateLogin(string loginToUpdate, [FromBody] UpdateLoginDto loginRequest, [FromHeader(Name = "ClientInfo")] string loginWhoUpdates, CancellationToken cancellationToken = default)
        {
            try
            {
                var userToUpdate = await _userService.UpdateLogin(loginToUpdate, loginRequest, loginWhoUpdates, cancellationToken);
                return Ok(userToUpdate);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("active-users")]
        [RoleFilter(Roles.AdminOnly)]
        public async Task<ActionResult<List<UserDto>>> GetActiveUsers([FromHeader(Name = "ClientInfo")] string loginWhoRequests, CancellationToken cancellationToken = default)
        {
            try
            {
                var users = await _userService.GetActiveUsers(loginWhoRequests, cancellationToken);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user-by-login/{loginToGet}")]
        [RoleFilter(Roles.AdminOnly)]
        public async Task<ActionResult<GetPersonalDataDto>> GetUserByLogin(string loginToGet, [FromHeader(Name = "ClientInfo")] string loginWhoRequests, CancellationToken cancellationToken = default)
        {
            try
            {
                var user = await _userService.GetUserByLogin(loginToGet, loginWhoRequests, cancellationToken);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("me-by-login-and-password")]
        [RoleFilter(Roles.SelfOnly)]
        public async Task<ActionResult<UserDto>> GetUser([FromQuery] GetUserByLoginAndPasswordDto loginAndPasswordToGet, [FromHeader(Name = "ClientInfo")] string whoRequestedLogin, CancellationToken cancellationToken = default)
        {
            try
            {
                var user = await _userService.GetUser(loginAndPasswordToGet, whoRequestedLogin, cancellationToken);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("users-older-than/{overTheAgeOf}")]
        [RoleFilter(Roles.AdminOnly)]
        public async Task<ActionResult<List<UserDto>>> GetUsersOverTheAgeOf(int overTheAgeOf, [FromHeader(Name = "ClientInfo")] string loginWhoRequests, CancellationToken cancellationToken = default)
        {
            try
            {
                var users = await _userService.GetUsersOverTheAgeOf(overTheAgeOf, loginWhoRequests, cancellationToken);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("revoke-user/{loginToRevoke}")]
        [RoleFilter(Roles.AdminOnly)]
        public async Task<ActionResult<UserDto>> RevokeUser(string loginToRevoke, [FromHeader(Name = "ClientInfo")] string loginWhoDeletes, CancellationToken cancellationToken = default)
        {
            try
            {
                var revokedUser = await _userService.RevokeUser(loginToRevoke, loginWhoDeletes, cancellationToken);
                return Ok(revokedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("restore-user/{loginToRestore}")]
        [RoleFilter(Roles.AdminOnly)]
        public async Task<ActionResult<UserDto>> RestoreUser(string loginToRestore, [FromHeader(Name = "ClientInfo")] string loginWhoRestores, CancellationToken cancellationToken = default)
        {
            try
            {
                var restoredUser = await _userService.RestoreUser(loginToRestore, loginWhoRestores, cancellationToken);
                return Ok(restoredUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}