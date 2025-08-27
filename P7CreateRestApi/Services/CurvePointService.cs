using P7CreateRestApi.Entities;
using P7CreateRestApi.Repositories; // <-- pour ICurvePointRepository

namespace P7CreateRestApi.Services
{
    public class CurvePointService
    {
        private readonly ICurvePointRepository _repository; // <-- interface

        public CurvePointService(ICurvePointRepository repository) // <-- interface ici aussi
        {
            _repository = repository;
        }

        public Task<List<CurvePoint>> GetAllAsync() => _repository.GetAllAsync();

        public Task<CurvePoint?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task<CurvePoint> CreateAsync(CurvePoint curve) => _repository.AddAsync(curve);

        public Task<bool> UpdateAsync(int id, CurvePoint curve) => _repository.UpdateAsync(id, curve);

        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}
