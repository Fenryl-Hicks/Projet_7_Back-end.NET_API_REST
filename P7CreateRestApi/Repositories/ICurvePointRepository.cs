using P7CreateRestApi.Entities;

namespace P7CreateRestApi.Repositories
{
    public interface ICurvePointRepository
    {
        Task<CurvePoint> AddAsync(CurvePoint curve);
        Task<bool> DeleteAsync(int id);
        Task<List<CurvePoint>> GetAllAsync();
        Task<CurvePoint?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, CurvePoint updated);
    }
}