﻿using BlazorSozluk.Api.Application.Interfaces;
using BlazorSozluk.Infrastructure.Persistence.Context;
using BlazorSozluk.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Infrastructure.Persistence.Extensions
{
    public static class Registration
    {
        public static IServiceCollection AddInfrastructureRegistration(
            this IServiceCollection services, IConfiguration configuration
            )
        {
            services.AddDbContext<BlazorSozlukContext>(conf =>
            {
                var constr = configuration["BlazorSozlukDbConnectionString"].ToString();
                conf.UseSqlServer(constr, opt =>
                {

                    opt.EnableRetryOnFailure();
                });
            });

            var seedData = new SeedData();
            seedData.SeedAsync(configuration).GetAwaiter().GetResult();
            services.AddScoped<IUserRepository, UserRepository>();//biri senden IUserRepository isterse ona UserRepository ı ver
            services.AddScoped<IEmailConfirmationRepository, EmailConfirmationRepository>();
            services.AddScoped<IEntryRepository, EntryRepository>();
            services.AddScoped<IEntryCommentRepository, EntryCommentRepostory>();
            return services;
        }
    }
}
