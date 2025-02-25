﻿using BadmintonSocialNetwork.API.Middlewares;

namespace BadmintonSocialNetwork.API.Extensions
{
    public static class MiddlewareExtension
    {
        public static WebApplication UseJwtMiddleware(this WebApplication app)
        {
            app.UseMiddleware<JwtMiddleware>();
            return app;
        }

        public static WebApplication UseErrorHandlingMiddle(this WebApplication app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
            return app;
        }
    }
}
