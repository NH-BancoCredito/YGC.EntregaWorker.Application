using MediatR;
using EntregaWorker.Domain.Service.Events;
using static Confluent.Kafka.ConfigPropertyNames;
using System.Threading;
using Microsoft.AspNetCore.Components.Forms;
using ThirdParty.Json.LitJson;
using Newtonsoft.Json;
using EntregaWorker.Application.CasosUso.AdministrarEntregas.RegistarEntrega;
using Amazon.Runtime.Internal;
using Newtonsoft.Json.Linq;
using System.Net;
using EntregaWorker.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
//using Venta.Worker.Repositories;
//using Venta.Domain.Services.WebServices;

namespace Entrega.Worker.Workers
{
    public class ActualizarStocksWorker : BackgroundService
    {
        private readonly IConsumerFactory _consumerFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configInfo;


        public ActualizarStocksWorker(IConsumerFactory consumerFactory, IServiceProvider serviceProvider, IConfiguration configInfo)
        {
            _consumerFactory = consumerFactory;
            _serviceProvider = serviceProvider;
            _configInfo = configInfo;


        }
        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var consumer = _consumerFactory.GetConsumer();
            consumer.Subscribe("entregas");
            var dado1 = _configInfo["dbEntregas-cnx"];

            while (!cancellationToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var consumeResult = consumer.Consume(cancellationToken);
                if (consumeResult.Value != null)
                {
                    var request = consumeResult.Value;

                    var objEntrega = JsonConvert.DeserializeObject<RegistrarEntregaRequest>(request);
                   
                    var response =  _mediator.Send(objEntrega);
                }
            }

            consumer.Close();

            return Task.CompletedTask;
        }
    }
}
