using P7CreateRestApi.Entities;
using P7CreateRestApi.Repositories; // pour IRatingRepository

namespace P7CreateRestApi.Services
{
    public class RatingService
    {
        private readonly IRatingRepository _repository; // <-- interface

        public RatingService(IRatingRepository repository) // <-- interface ici aussi
        {
            _repository = repository;
        }

        public Task<List<Rating>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<Rating?> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<Rating> CreateAsync(Rating rating)
        {
            return _repository.AddAsync(rating);
        }

        public Task<bool> UpdateAsync(int id, Rating rating)
        {
            return _repository.UpdateAsync(id, rating);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }
    }
}
