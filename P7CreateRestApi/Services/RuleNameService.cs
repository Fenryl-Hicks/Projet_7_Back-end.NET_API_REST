using P7CreateRestApi.Entities;
using P7CreateRestApi.Repositories; 

namespace P7CreateRestApi.Services
{
    public class RuleNameService
    {
        private readonly IRuleNameRepository _repository;

        public RuleNameService(IRuleNameRepository repository)
        {
            _repository = repository;
        }

        public Task<List<RuleName>> GetAllAsync() => _repository.GetAllAsync();

        public Task<RuleName?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task<RuleName> CreateAsync(RuleName rule) => _repository.AddAsync(rule);

        public Task<bool> UpdateAsync(int id, RuleName rule) => _repository.UpdateAsync(id, rule);

        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}
