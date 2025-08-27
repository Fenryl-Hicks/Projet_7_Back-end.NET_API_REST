using P7CreateRestApi.Entities;
using P7CreateRestApi.Repositories;

namespace P7CreateRestApi.Services
{
    public class BidService
    {
        private readonly IBidRepository _repository; // <-- interface

        public BidService(IBidRepository repository) // <-- interface ici aussi
        {
            _repository = repository;
        }

        public Task<List<Bid>> GetAllAsync() => _repository.GetAllAsync();

        public Task<Bid?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task<Bid> CreateAsync(Bid bid) => _repository.AddAsync(bid);

        public Task<bool> UpdateAsync(int id, Bid bid) => _repository.UpdateAsync(id, bid);

        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}
