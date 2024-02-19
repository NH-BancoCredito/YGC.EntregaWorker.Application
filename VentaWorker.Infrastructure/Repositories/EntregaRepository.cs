using MongoDB.Driver;
using MongoDB.Driver.Linq;
using EntregaWorker.Domain.Models;
using EntregaWorker.Domain.Repositories;

namespace EntregaWorker.Infrastructure.Repositories
{
    public class EntregaRepository : IEntregaRepository
    {
        private readonly IMongoDatabase _mongoDatabase;

        public EntregaRepository(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
        }

        public async Task<bool> Adicionar(Entrega entity)
        {
            await GetMongoCollection().InsertOneAsync(entity);

            return true;
        }

        public async Task<Entrega> Consultar(int id)
        {
            return await GetMongoCollection().FindSync<Entrega>(item => item.IdVenta == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Entrega>> Consultar(string nombre)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Eliminar(Entrega entity)
        {
            throw new NotImplementedException();
        }

        public async  Task<bool> Modificar(Entrega entity)
        {
            await GetMongoCollection().ReplaceOneAsync(item => item.IdVenta == entity.IdVenta, entity);

            return true;
        }

        private IMongoCollection<Entrega> GetMongoCollection() => _mongoDatabase.GetCollection<Entrega>(nameof(Entrega));
    }
}
