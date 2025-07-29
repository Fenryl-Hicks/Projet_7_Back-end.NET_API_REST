using P7CreateRestApi.Entities;
using P7CreateRestApi.Repositories;

namespace P7CreateRestApi.Services
{
    public class CurvePointService
    {
        private readonly CurvePointRepository _repository;

        public CurvePointService(CurvePointRepository repository)
        {
            _repository = repository;
        }

        public Task<List<CurvePoint>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<CurvePoint?> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<CurvePoint> CreateAsync(CurvePoint curve)
        {
            return _repository.AddAsync(curve);
        }

        public Task<bool> UpdateAsync(int id, CurvePoint curve)
        {
            return _repository.UpdateAsync(id, curve);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }
    }
}
