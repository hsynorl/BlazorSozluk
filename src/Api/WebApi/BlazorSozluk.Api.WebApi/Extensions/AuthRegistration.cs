﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BlazorSozluk.Api.WebApi.Extensions
{
    public static class AuthRegistration
    {
        public static IServiceCollection ConfigureAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var singingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["AuthConfig:Secret"]));

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = singingKey
                };
            });

            return services;
        }
    }
}
