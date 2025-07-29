using P7CreateRestApi.Entities;
using P7CreateRestApi.Repositories;

namespace P7CreateRestApi.Services
{
    public class RuleNameService
    {
        private readonly RuleNameRepository _repository;

        public RuleNameService(RuleNameRepository repository)
        {
            _repository = repository;
        }

        public Task<List<RuleName>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<RuleName?> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<RuleName> CreateAsync(RuleName rule)
        {
            return _repository.AddAsync(rule);
        }

        public Task<bool> UpdateAsync(int id, RuleName rule)
        {
            return _repository.UpdateAsync(id, rule);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }
    }
}
