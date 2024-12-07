﻿using System.Diagnostics.CodeAnalysis;
using Hangfire;
using Hangfire.Console;
using Hangfire.Redis.StackExchange;
using Infra.Data;
using Infra.Utils.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.IoC
{
    [ExcludeFromCodeCoverage]
    public static class NativeInjector
    {
        public static void AddLocalHttpClients(this IServiceCollection services, IConfiguration configuration) {}

        public static void AddLocalServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region Services
            #endregion

            #region Repositories
            #endregion
        }

        public static void AddLocalUnitOfWork(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Context>(options => options.UseLazyLoadingProxies().UseNpgsql(Builders.BuildPostgresConnectionString(configuration)));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void AddLocalCache(this IServiceCollection services, IConfiguration configuration) {
            services.AddStackExchangeRedisCache(options => options.Configuration = Builders.BuildRedisConnectionString(configuration));
        }

        public static void AddLocalHangfire(this IServiceCollection services, IConfiguration configuration) {
            services.AddHangfire(options => {
                options.UseRedisStorage(Builders.BuildRedisConnectionString(configuration), new RedisStorageOptions { Prefix = "HANGFIRE" });
                options.UseConsole();
            });
            services.AddHangfireServer();
        }

        public static void AddLocalHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddNpgSql(Builders.BuildPostgresConnectionString(configuration))
                .AddRedis(Builders.BuildRedisConnectionString(configuration));
        }
    }
}
