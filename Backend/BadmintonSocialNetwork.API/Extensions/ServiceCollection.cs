using AutoMapper;
using BadmintonSocialNetwork.Repository.Data;
using BadmintonSocialNetwork.Repository.Implements;
using BadmintonSocialNetwork.Repository.Interfaces;
using BadmintonSocialNetwork.Service.Services;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSocialNetwork.API.Extensions
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtTokenFactory, JwtTokenFactory>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            return services;
        }

        private static string GetConnectionString()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            return config["ConnectionStrings:DefaultConnection"];
        }
    }
}
