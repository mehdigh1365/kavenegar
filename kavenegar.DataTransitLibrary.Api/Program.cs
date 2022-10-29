using Kavenegar.DataTransitLibrary.Common.Options;
using Kavenegar.DataTransitLibrary.Persistence.Configurations;
using Kavenegar.DataTransitLibrary.Persistence.Context;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace kavenegar.DataTransitLibrary.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", false, true)
               .Build();

            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                #region Database
                var options = new DataSetting();
                config.GetSection(nameof(DataSetting)).Bind(options);
                if (options.AutoMigration == true)
                {
                    var context = services.GetRequiredService<DataTransitLibraryContext>();
                    context.Database.SetCommandTimeout(options.MigrationCommandTimout);
                    await context.Database.MigrateAsync();
                }
                #endregion Database

                await host.RunAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                //Serilog Add
                //Log.Error(ex, ex.Message);
            }
            finally
            {
                //Serilog Add
                //Log.CloseAndFlush();
            }
          
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
