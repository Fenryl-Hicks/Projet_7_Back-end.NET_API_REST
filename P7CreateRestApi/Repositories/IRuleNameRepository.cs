using P7CreateRestApi.Entities;

namespace P7CreateRestApi.Repositories
{
    public interface IRuleNameRepository
    {
        Task<RuleName> AddAsync(RuleName rule);
        Task<bool> DeleteAsync(int id);
        Task<List<RuleName>> GetAllAsync();
        Task<RuleName?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, RuleName updated);
    }
}