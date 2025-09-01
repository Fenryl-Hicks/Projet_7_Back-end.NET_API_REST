using P7CreateRestApi.Entities;
using P7CreateRestApi.Repositories; 

namespace P7CreateRestApi.Services
{
    public class RatingService
    {
        private readonly IRatingRepository _repository; 

        public RatingService(IRatingRepository repository) 
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
