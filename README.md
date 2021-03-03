# Gravação de log no ElasticSearch com .NET 5

Este projeto implementa um console application em .NET 5 que grava logs em uma instância do ElasticSeach fazendo o uso da biblioteca Serilog.

É demostrado também uma forma de customização os logs, com a inclusão das propriedades Hostname, AssemblyFullPath e AssemblyName. Posteriormente, no Kibana, é possível aplicar filtros e funções de agregação nestas propriedades.

## Pré-requisitos

 - .NET 5 SDK (https://dotnet.microsoft.com/download/dotnet/5.0)
 - Docker (https://www.docker.com/products/docker-desktop)
 

## Processo

### Iniciar os serviços do ElasticSearch e Kibana

No diretório onde está o arquivo *docker-compose.yml*, executar o comando a seguir:

    docker-compose up -d

Levará alguns segundos para os serviços serem iniciados. Quando o processo for finalizado, será possível acessar o Kibana.

    http://localhost:5601
    login: elastic | pwd: elastic@123

### Executar o console para iniciar a gravação de logs

No diretório .\src digitar os comandos a seguir:

    dotnet restore
    dotnet run
    

### Visualização dos logs no Kibana

No Kibana, clicar no ícone hamburger e em *Analytics* acessar a opção *Discover*.

Clicar no botão *Create index pattern* e configurar o padrão *logstash-AAAA.MM.DD*

Clicar novamente no ícone hamburger e em *Analytics* acessar a opção *Discover* e os logs serão exibidos.

[Instruções em formato PDF com os prints das telas](https://github.com/haroldo-rg/dotnet-log-elasticsearch/blob/main/passo-a-passo.pdf)

## Palavras-chave

dotnet | dotnet5 | .net5 | .netcore | dotnetcore | elasticasearch | kibana | docker | docker-compose | serilog | customlogs | logevent.addorupdateproperty