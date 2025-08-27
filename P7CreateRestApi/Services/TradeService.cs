using P7CreateRestApi.Entities;
using P7CreateRestApi.Repositories; 

namespace P7CreateRestApi.Services
{
    public class TradeService
    {
        private readonly ITradeRepository _repository;

        public TradeService(ITradeRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Trade>> GetAllAsync() => _repository.GetAllAsync();

        public Task<Trade?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task<Trade> CreateAsync(Trade trade) => _repository.AddAsync(trade);

        public Task<bool> UpdateAsync(int id, Trade trade) => _repository.UpdateAsync(id, trade);

        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}
