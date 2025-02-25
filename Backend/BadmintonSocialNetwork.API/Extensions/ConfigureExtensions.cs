using BadmintonSocialNetwork.Service.Settings;

namespace BadmintonSocialNetwork.API.Extensions
{
    public static class ConfigureExtensions
    {
        public static IServiceCollection ConfigureSection(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            return services;
        }


    }
}
