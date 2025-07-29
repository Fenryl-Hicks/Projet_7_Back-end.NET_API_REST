using P7CreateRestApi.Entities;

namespace P7CreateRestApi.Repositories
{
    public interface IBidRepository
    {
        Task<Bid> AddAsync(Bid bid);
        Task<bool> DeleteAsync(int id);
        Task<List<Bid>> GetAllAsync();
        Task<Bid?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, Bid bid);
    }
}