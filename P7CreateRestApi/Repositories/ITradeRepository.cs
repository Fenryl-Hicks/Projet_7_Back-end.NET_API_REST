using P7CreateRestApi.Entities;

namespace P7CreateRestApi.Repositories
{
    public interface ITradeRepository
    {
        Task<Trade> AddAsync(Trade trade);
        Task<bool> DeleteAsync(int id);
        Task<List<Trade>> GetAllAsync();
        Task<Trade?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, Trade updated);
    }
}