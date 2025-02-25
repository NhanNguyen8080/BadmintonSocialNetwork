using BadmintonSocialNetwork.API.Attributes;
using BadmintonSocialNetwork.Service.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace BadmintonSocialNetwork.API.Handlers
{
    public class CustomAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            IJwtTokenFactory jwtTokenFactory) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
    {

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var endpoint = Context.GetEndpoint();
            //keeping RolesAuthorize Attributes ([RolesAuthorize("User")])
            var rolesAuthorize = endpoint?.Metadata.GetMetadata<RolesAuthorizeAttribute>();

            if (rolesAuthorize is null)
            {
                return AuthenticateResult.NoResult(); // Skip authentication if not required
            }

            var authHeader = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader) || 
                !authHeader.StartsWith("Bearer ", System.StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticateResult.Fail("Token is missing");
            }

            var token = GetTokenFromHeader(authHeader);
            if (string.IsNullOrEmpty(token))
            {
                return AuthenticateResult.Fail("Token is missing");
            }

            var principal = jwtTokenFactory.ValidateToken(token);
            if (principal is null)
            {
                return AuthenticateResult.Fail("Invalid token");
            } else
            {
                return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
            }

        }

        private string? GetTokenFromHeader(string authHeader)
        {
            return authHeader.Split(" ").Last();
        }
    }
}
