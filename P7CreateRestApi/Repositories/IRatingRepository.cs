using P7CreateRestApi.Entities;


namespace P7CreateRestApi.Repositories;

public interface IRatingRepository
{
    Task<Rating> AddAsync(Rating rating);
    Task<bool> DeleteAsync(int id);
    Task<List<Rating>> GetAllAsync();
    Task<Rating?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(int id, Rating updated);
}