using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntregaWorker.Domain.Models;

namespace EntregaWorker.Domain.Repositories
{
    public  interface IEntregaRepository : IRepository
    {
        Task<bool> Adicionar(Entrega entity);

        Task<bool> Modificar(Entrega entity);

        Task<bool> Eliminar(Entrega entity);

        Task<Entrega> Consultar(int id);

        Task<IEnumerable<Entrega>> Consultar(string nombre);


    }
}
