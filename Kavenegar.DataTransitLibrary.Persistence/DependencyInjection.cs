using Kavenegar.DataTransitLibrary.Common.Interfaces;
using Kavenegar.DataTransitLibrary.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kavenegar.DataTransitLibrary.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataTransitLibraryContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DataTransitLibraryContext"), sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        10, TimeSpan.FromSeconds(30), null);

                });

            }, ServiceLifetime.Transient);

            services.AddTransient<IDataTransitLibraryContext, DataTransitLibraryContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
