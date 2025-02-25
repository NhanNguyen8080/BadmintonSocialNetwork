using BadmintonSocialNetwork.API.Handlers;
using Microsoft.AspNetCore.Authentication;

namespace BadmintonSocialNetwork.API.Extensions
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddJwtAuth(this IServiceCollection services)
        {
            services.AddAuthentication("Scheme")
                .AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>("Scheme", null);
            services.AddAuthorization();
            return services;
        }
    }
}
