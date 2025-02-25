using BadmintonSocialNetwork.Repository.Data;
using BadmintonSocialNetwork.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSocialNetwork.API.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddDatabaseContext(this IServiceCollection services)
        {
            services.AddDbContext<BadmintonSocialNetworkDBContext>(options =>
            {
                options.UseNpgsql(GetConnectionString(), b => b.MigrationsAssembly("BadmintonSocialNetwork.API"));
            }, ServiceLifetime.Scoped);

            //services.AddStackExchangeRedisCache(options =>
            //{
            //    options.Configuration = GetConnectionStringsRedis();
            //    options.InstanceName = "BadmintonSocialNetwork";
            //});
            return services;
        }

        public static WebApplication SeedDatabase(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BadmintonSocialNetworkDBContext>();
                if (!context.Roles.Any())
                {
                    var adminRole = new Role { RoleName = "Admin" };
                    var userRole = new Role { RoleName = "User" };

                    context.Roles.AddRange(adminRole, userRole);

                    context.SaveChanges();
                }
            }
            return app;
        }

        private static string GetConnectionString()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            return config["ConnectionStrings:DefaultConnection"];
        }

        private static string GetConnectionStringsRedis()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var strConn = config["ConnectionStrings:Redis"];
            return strConn;
        }
    }
}
