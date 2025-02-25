using Microsoft.AspNetCore.Mvc.Filters;

namespace BadmintonSocialNetwork.API.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RolesAuthorizeAttribute(params string[] roles) : Attribute, IAuthorizationFilter
    {
        public string[] Roles { get; } = roles;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
        }
    }
}
