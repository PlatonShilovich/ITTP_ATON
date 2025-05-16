using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApi_ITTP_ATON.Enums;
using WebApi_ITTP_ATON.Repositories;

namespace WebApi_ITTP_ATON.Filters
{
    public class RoleFilter : IAsyncAuthorizationFilter
    {
        private readonly Roles _requiredRole;

        public RoleFilter(Roles requiredRole)
        {
            _requiredRole = requiredRole;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var cancellationToken = context.HttpContext.RequestAborted;

            var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();

            var loginWhoRequests = context.HttpContext.Request.Headers["ClientInfo"].ToString();
            if (string.IsNullOrEmpty(loginWhoRequests))
            {
                context.Result = new ObjectResult(new { error = "ClientInfo header is missing" }) { StatusCode = 401 };
                return;
            }
            
            var user = await userRepository.GetUserByLogin(loginWhoRequests, cancellationToken);
            if (user == null)
            {
                context.Result = new ObjectResult(new { error = "User not found" }) { StatusCode = 401 };
                return;
            }

            if (_requiredRole != Roles.AdminOnly && !user.Admin && user.RevokedOn != DateTime.MinValue && user.RevokedBy != string.Empty)
            {
                context.Result = new ObjectResult(new { error = "User is revoked" }) { StatusCode = 401 };
                return;
            }

            switch (_requiredRole)
            {
                case Roles.AdminOnly:
                    if (!user.Admin)
                    {
                        context.Result = new ObjectResult(new { error = "User is not an admin" }) { StatusCode = 401 };
                    }
                    break;

                case Roles.SelfOnly:
                    var loginToGet = GetLoginFromRouteOrQuery(context);
                    if (loginToGet != loginWhoRequests)
                    {
                        context.Result = new ObjectResult(new { error = "Access denied: action allowed only for self" }) { StatusCode = 401 };
                    }
                    break;

                case Roles.AdminOrSelf:
                    var targetLogin = GetLoginFromRouteOrQuery(context);
                    if (!user.Admin && targetLogin != loginWhoRequests)
                    {
                        context.Result = new ObjectResult(new { error = "Access denied: action allowed only for admin or self" }) { StatusCode = 401 };
                    }
                    break;
            }
        }

        private string GetLoginFromRouteOrQuery(AuthorizationFilterContext context)
        {
            if (context.RouteData.Values.TryGetValue("loginToUpdate", out var login) && login != null)
            {
                return login.ToString()!;
            }
            if (context.RouteData.Values.TryGetValue("loginToGet", out login) && login != null)
            {
                return login.ToString()!;
            }
            if (context.RouteData.Values.TryGetValue("loginToDelete", out login) && login != null)
            {
                return login.ToString()!;
            }
            if (context.RouteData.Values.TryGetValue("loginToRestore", out login) && login != null)
            {
                return login.ToString()!;
            }

            var queryLogin = context.HttpContext.Request.Query["login"];
            if (!string.IsNullOrEmpty(queryLogin))
            {
                return queryLogin.ToString()!;
            }

            throw new InvalidOperationException("Login parameter not found in route or query.");
        }
    }
}