﻿using BadmintonSocialNetwork.API.Filters;
using Microsoft.OpenApi.Models;

namespace BadmintonSocialNetwork.API.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            //This setup is useful if API uses JWT-based authentication and test it easily in Swagger.
            services.AddSwaggerGen(options =>
            {
                options.SchemaFilter<DateOnlySchemaFilter>();
                //This defines the security scheme that Swagger will use for authentication.
                options.AddSecurityDefinition("Bearer" /* The name of the security scheme */, 
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization", //The name of the header where the token will be passed
                        Type = SecuritySchemeType.Http, //Specifies that this is an HTTP authentication scheme.
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Fill in the JWT Token that follows this format (Bearer [token])",
                    });
                //This enforces the security requirement globally,
                //meaning all API endpoints will require a Bearer token for authentication.
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                    
                });
            });
            return services;
        }

        public static WebApplication UseSwaggerDocumentation(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            return app;
        }
    }
}
