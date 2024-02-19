using AutoMapper;
using MediatR;
using EntregaWorker.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntregaWorker.Application;
using EntregaWorker.Domain.Models;
using EntregaWorker.Domain.Repositories;

namespace EntregaWorker.Application.CasosUso.AdministrarEntregas.RegistarEntrega
{
    /* 
     1 - Deberia verificar si el productos
     Si existe , entoces actualizar en la table de producto
     Si no existe, crear un nuevo registro

     */
    public class RegistrarEntregaHandler :
       IRequestHandler<RegistrarEntregaRequest, IResult>
    {
        private readonly IEntregaRepository _entregaRepository;
        private readonly IMapper _mapper;

        public RegistrarEntregaHandler(IEntregaRepository entregaRepository, IMapper mapper)
        {
            _entregaRepository = entregaRepository;
            _mapper = mapper;
        }


        public async Task<IResult> Handle(RegistrarEntregaRequest request, CancellationToken cancellationToken)
        {

            IResult response = null;
            bool result = false;

            try
            {
                var entrega = _mapper.Map<Entrega>(request);
                await _entregaRepository.Adicionar(entrega);

                if (result)
                {
                    return new SuccessResult();
                }
                else
                    return new FailureResult();

            }
            catch (Exception ex)
            {
                response = new FailureResult();
                return response;
            }
        }
    }
}
