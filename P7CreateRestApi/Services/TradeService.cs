using P7CreateRestApi.Entities;
using P7CreateRestApi.Repositories;

namespace P7CreateRestApi.Services
{
    public class TradeService
    {
        private readonly TradeRepository _repository;

        public TradeService(TradeRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Trade>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<Trade?> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<Trade> CreateAsync(Trade trade)
        {
            // Ici tu peux ajouter des règles métier si besoin
            return _repository.AddAsync(trade);
        }

        public Task<bool> UpdateAsync(int id, Trade trade)
        {
            return _repository.UpdateAsync(id, trade);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }
    }
}
