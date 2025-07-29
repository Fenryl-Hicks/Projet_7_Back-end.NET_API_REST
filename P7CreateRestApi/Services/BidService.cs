using P7CreateRestApi.Entities;
using P7CreateRestApi.Repositories;

namespace P7CreateRestApi.Services
{
    public class BidService
    {
        private readonly BidRepository _repository;

        public BidService(BidRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Bid>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<Bid?> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<Bid> CreateAsync(Bid bid)
        {
            // Ici tu peux insérer des règles métier (ex: vérifier Account non vide)
            return _repository.AddAsync(bid);
        }

        public Task<bool> UpdateAsync(int id, Bid bid)
        {
            return _repository.UpdateAsync(id, bid);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }
    }
}
