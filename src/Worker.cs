using System;
using Microsoft.Extensions.Logging;
using LogElastic.Interface;

namespace LogElastic
{
    public class Worker: IWorker
    {
        private readonly ILogger<Worker> _logger;
        public string ProcessId { get; set; }

        public Worker(ILogger<Worker> logger)
        {
            this._logger = logger;
            this.ProcessId = Guid.NewGuid().ToString();
        }

        public bool RunProcess()
        {
            _logger.LogInformation($"Processamento inciado. { this.ToString() }");

            try
            {
                // a condição para simular erro é segundos da hora atual serem multiplo de 5
                if( (DateTime.Now.Second % 5 == 0) )
                    throw new Exception("Ocorreu um erro no processamento");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ocorreu um erro no processamento { this.ToString() }");

                return false;
            }
            finally{
                _logger.LogInformation($"Processamento finalizado. { this.ToString() }");
            }
        }

        public override string ToString()
        {
            return $"ProcessId: {ProcessId}";
        }
    }
}