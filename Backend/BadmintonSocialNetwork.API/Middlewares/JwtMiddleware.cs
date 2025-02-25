using BadmintonSocialNetwork.API.Attributes;
using System.Security.Claims;

namespace BadmintonSocialNetwork.API.Middlewares
{
    public class JwtMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;
        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var rolesAuthorize = endpoint?.Metadata.GetMetadata<RolesAuthorizeAttribute>();

            if (rolesAuthorize is not null)
            {
                if (context.User.Identity?.IsAuthenticated == true)
                {
                    var userRoles = context.User.FindAll(ClaimTypes.Role).Select(_ => _.Value);
                    if (!rolesAuthorize.Roles.Any(role => userRoles.Contains(role)))
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("Forbidden: You do not have access to this resource.");
                        return;
                    }
                } else
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized: You need to login to access this resource.");
                    return;
                }
            }

            await _next(context);
            return;

        }
    }
}
