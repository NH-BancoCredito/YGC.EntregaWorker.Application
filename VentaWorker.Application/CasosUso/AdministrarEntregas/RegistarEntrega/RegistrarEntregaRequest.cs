using EntregaWorker.Domain.Models;
using MediatR;
using EntregaWorker.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntregaWorker.Application.CasosUso.AdministrarEntregas.RegistarEntrega
{
    public class RegistrarEntregaRequest : IRequest<IResult>
    {
        public object Id { get; set; }
        public int IdVenta { get; set; }
        public string NombreCliente { get; set; }
        public string DireccionEntrega { get; set; }
        public string Ciudad { get; set; }
        public virtual IEnumerable<EntregaDetalleRequest> Detalle { get; set; }
    }
    public class EntregaDetalleRequest
    {
        public int IdEntregaDetalle { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
    }
}
