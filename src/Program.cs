using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using LogElastic.Interface;

namespace LogElastic
{
    class Program
    {
        static void Main(string[] args)
        {
            // configurar injeção de dependência dos serviços
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection, args);
            var serviceProvider = serviceCollection.BuildServiceProvider();


            // INICIO: PROCESSAMENTO

            // instanciar um objeto de log da classe Program
            var logger = serviceProvider.GetService<ILogger<Program>>();
            logger.LogInformation("Console inciado");

            Console.WriteLine("Pressionale ctrl + c para sair");

            while(true)            
            {
                // instancia um objeto da classe que efetua o processamento
                var process = serviceProvider.GetService<IWorker>();

                // inicia o processamento
                bool sucesso = process.RunProcess();

                Console.WriteLine(String.Format("{0} - Processado com {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), sucesso ? "sucesso" : "erro"));

                // aguarda 5 segundos antes de iniciar o próximo processamento
                System.Threading.Thread.Sleep(3000);
            }

            // FIM : PROCESSAMENTO

        }


        private static void ConfigureServices(IServiceCollection services, string[] args)
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

            Log.Logger = new LoggerConfiguration()
                //.Enrich.FromLogContext()
                .Enrich.With(new MyCustomLogEnricher()) // classe customizada que adiciona propriedades ao log
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticConfig.Uri))
                {
                    AutoRegisterTemplate = true,
                    ModifyConnectionSettings = x => x.BasicAuthentication(elasticConfig.UserName, elasticConfig.Password),
                })
            .CreateLogger();

            services.AddLogging(configure => configure.AddSerilog())
                    .AddTransient<Program>();

            // Configuração do service serviço que efetua o processamento
            services.AddTransient(typeof(IWorker), typeof(Worker));
        }

    }
}
