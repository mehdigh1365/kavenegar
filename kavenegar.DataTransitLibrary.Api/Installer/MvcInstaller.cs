using AutoMapper;
using Hangfire;
using Hangfire.SqlServer;
using kavenegar.DataTransitLibrary.Application.Common.AutoMapper;
using Kavenegar.DataTransitLibrary.Common.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace kavenegar.DataTransitLibrary.Api.Installer
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy",
                    builder => { builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod(); });
            });

            #region add Dependency

            services.Configure<RedisConfiguration>(configuration.GetSection("RedisConfiguration"));


            #endregion add Dependency

            #region Redis

            var redisConfiguration = new RedisConfiguration();
            configuration.Bind(nameof(RedisConfiguration), redisConfiguration);
            services.AddSingleton(redisConfiguration);

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = redisConfiguration.Connection;
            });

            #endregion Redis

            #region Automapper

            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            #endregion Automapper

            #region HangFire

            services.AddHangfire(conf =>
            {
                conf
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(configuration.GetConnectionString("HangFireConnection"),
                        new SqlServerStorageOptions
                        {
                            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                            QueuePollInterval = TimeSpan.Zero,
                            UseRecommendedIsolationLevel = true,
                            UsePageLocksOnDequeue = true,
                            DisableGlobalLocks = true,
                        });


            }
            );



            #endregion HangFire

            #region Swagger

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Kavenegar",
                    Version = "v1.0",
                    Description = "Kavenegar ASP.NET Core Web Api",
                });
            });

            #endregion Swagger
        }
    }
}
