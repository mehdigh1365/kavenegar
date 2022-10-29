using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace kavenegar.DataTransitLibrary.Api.Installer
{
    public interface IInstaller
    {
        void InstallServices(IConfiguration configuration, IServiceCollection services);
    }
}
