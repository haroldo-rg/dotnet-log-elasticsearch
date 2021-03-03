using System;
using LogElastic.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace LogElastic.Configuration
{
    public class MyLoggerConfiguration : ILoggerConfiguration
    {
        public void ConfigureLogServices<T>(IServiceCollection services, string[] args) where T: class
        {
            // Configuração do log com ElasticSearch
            IConfiguration Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            ElasticConfiguration elasticConfig = new ElasticConfiguration(
                                                        Configuration["ElasticConfiguration:Uri"],
                                                        Configuration["ElasticConfiguration:UserName"],
                                                        Configuration["ElasticConfiguration:Password"]
                                                    );

            // configura o serviço de log para gravar no ElasticSearch e em arquivo .txt local
            Log.Logger = new LoggerConfiguration()
                //.Enrich.FromLogContext()
                .Enrich.With(new MyCustomLogEnricher()) // classe customizada que adiciona propriedades ao log
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticConfig.Uri))
                {
                    AutoRegisterTemplate = true,
                    ModifyConnectionSettings = x => x.BasicAuthentication(elasticConfig.UserName, elasticConfig.Password),
                })
                .WriteTo.File(
                    path: Configuration["SeriLog:FilePath"], 
                    rollingInterval: (RollingInterval)Convert.ToUInt32(Configuration["SeriLog:RollingInterval"]), 
                    outputTemplate: Configuration["SeriLog:FileOutputTemplate"])
            .CreateLogger();

            services.AddLogging(configure => configure.AddSerilog())
                    .AddTransient<T>();   
        }
    }
}