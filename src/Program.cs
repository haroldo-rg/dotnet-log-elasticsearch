using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LogElastic.Interface;
using LogElastic.Configuration;

namespace LogElastic
{
    class Program
    {
        static void Main(string[] args)
        {
            // configurar injeção de dependência dos serviços
            var serviceCollection = new ServiceCollection();
            ConfigureServices<Program>(serviceCollection, args);
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


        private static void ConfigureServices<T>(IServiceCollection services, string[] args) where T : class
        {
            // configuração do serviço de log
            new MyLoggerConfiguration().ConfigureLogServices<T>(services, args);

            // Configuração do service serviço que efetua o processamento
            services.AddTransient(typeof(IWorker), typeof(Worker));
        }

    }
}
