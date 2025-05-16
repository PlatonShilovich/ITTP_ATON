using Microsoft.AspNetCore.Mvc;
using WebApi_ITTP_ATON.Enums;
using WebApi_ITTP_ATON.Filters;

namespace WebApi_ITTP_ATON
{
    public class RoleFilterAttribute : TypeFilterAttribute
    {
        public RoleFilterAttribute(Roles requiredRole) : base(typeof(RoleFilter))
        {
            Arguments = new object[] { requiredRole };
        }
    }
}