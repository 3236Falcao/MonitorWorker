using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Monitor.Helpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.IO;

namespace Monitor
{
    public class Worker : BackgroundService
    {
        //Injeção de dependência
        private readonly ILogger<Worker> _logger;

        //variavel que recebe valores que só são da classe Sites
        private readonly Sites _sites;

        //Dentro do construtor chamar o Iconfiguration
        public Worker(ILogger<Worker> logger, IConfiguration _conf)
        {
            _logger = logger;
            _sites = _conf.GetSection("Sites").Get<Sites>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //colocar o status da informação dentro do while
            while (!stoppingToken.IsCancellationRequested)
            {
                HttpStatusCode status = await Requesters.GetStatusFromUrl(_sites.Url);

                //Se o status não for ok escreva o log
                if(status != HttpStatusCode.OK)
                {
                    //nomeFile recebe um nome e uma data do log
                    string nomeFile = string.Format("logfile_{0}.txt", DateTime.Now.ToString("yyyyMMdd"));
                    //path receb o caminho onde será salvo o log
                    string path = Path.Combine(@"C:\",nomeFile);
                    //o logFile recebe path, true porque sempre que for esse nome o log continua salvando nele
                    StreamWriter logFile = new StreamWriter(path, true);
                    string message = string.Format("O site {0} ficou fora do ar no dia {1}", _sites.Url, DateTime.Now.ToString());
                    logFile.WriteLine(message);
                    logFile.Close();
                }
                _logger.LogInformation("Worker rodando em: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
