using EntregaWorker.Domain.Repositories;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using System.Threading;
using NSubstitute.ExceptionExtensions;
using EntregaWorker.Application.CasosUso.AdministrarEntregas.RegistarEntrega;
//using static EntregaWorker.Application.CasosUso.AdministrarEntregas.RegistrarEntrega.RegistrarEntregaHandler;

namespace EntregaWorker.Test
{
    public class AdministrarEntregasTests
    {
        private readonly IEntregaRepository _entregaRepository;
        private readonly IMapper _mapper;
        private readonly RegistrarEntregaHandler _registrarEntregaHandler;

        public AdministrarEntregasTests()
        {
            _entregaRepository = Substitute.For<IEntregaRepository>();
            _mapper = Substitute.For<IMapper>();
            _registrarEntregaHandler = Substitute.For<RegistrarEntregaHandler>(_entregaRepository, _mapper);

        }

        
        [Fact]
        public async Task RegistrarEntregaError()
        {
            var request = new RegistrarEntregaRequest() { IdVenta = 1 };
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;
            _entregaRepository.Adicionar(default).ReturnsForAnyArgs(false);
            cts.Cancel();
            var retorno = await _registrarEntregaHandler.Handle(request, cancellationToken);
            Assert.False(retorno.HasSucceeded);

        }

        [Fact]
        public async Task RegistrarEntregaException()
        {
            var request = new RegistrarEntregaRequest() { IdVenta = 1 };
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;
            _entregaRepository.Adicionar(default).ThrowsForAnyArgs<Exception>();
            cts.Cancel();
            Assert.ThrowsAnyAsync<Exception>(async () =>
            {
                await _registrarEntregaHandler.Handle(request, cancellationToken);
            });


        }

    }
}