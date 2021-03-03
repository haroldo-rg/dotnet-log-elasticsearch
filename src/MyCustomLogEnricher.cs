using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Serilog.Core;
using Serilog.Events;
using LogElastic.Interface;

namespace LogElastic
{
    public class MyCustomLogEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            // altera o conteúdo da propriedade SourceContext
            var sourceContext = logEvent.Properties.GetValueOrDefault("SourceContext").ToString();
            sourceContext = $"Classe { sourceContext }";
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("SourceContext", sourceContext));

            // inclui a propriedade com o nome do host
            string hostName = System.Net.Dns.GetHostName();
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("Hostname", hostName));

            // inclui propriedades com informações do assembly
            string assemblyFullPath = Assembly.GetEntryAssembly().Location;
            string assemblyName = Path.GetFileName(assemblyFullPath);
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("AssemblyFullPath", assemblyFullPath));
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("AssemblyName", assemblyName));
        }        
    }
}