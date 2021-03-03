using Microsoft.Extensions.DependencyInjection;

namespace LogElastic.Interface
{
    interface ILoggerConfiguration
    {
        void ConfigureLogServices<T>(IServiceCollection services, string[] args) where T : class;
    }
}