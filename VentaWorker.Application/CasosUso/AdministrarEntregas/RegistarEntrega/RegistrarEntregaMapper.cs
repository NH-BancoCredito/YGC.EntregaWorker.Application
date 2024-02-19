using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EntregaWorker.Domain.Models;

namespace EntregaWorker.Application.CasosUso.AdministrarEntregas.RegistarEntrega
{
    public class RegistrarEntregaMapper : Profile
    {
        public RegistrarEntregaMapper()
        {
            CreateMap<RegistrarEntregaRequest, Entrega>()
                .ForMember(dest => dest.Detalle, map => map.MapFrom(src => src.Detalle));

            CreateMap<EntregaDetalleRequest, EntregaDetalle>();
        }
    }
}
