using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Data;
using P7CreateRestApi.Entities;

namespace P7CreateRestApi.Repositories
{
    public class RuleNameRepository : IRuleNameRepository
    {
        private readonly LocalDbContext _context;

        public RuleNameRepository(LocalDbContext context)
        {
            _context = context;
        }

        public async Task<List<RuleName>> GetAllAsync()
        {
            return await _context.RuleNames.ToListAsync();
        }

        public async Task<RuleName?> GetByIdAsync(int id)
        {
            return await _context.RuleNames.FindAsync(id);
        }

        public async Task<RuleName> AddAsync(RuleName rule)
        {
            _context.RuleNames.Add(rule);
            await _context.SaveChangesAsync();
            return rule;
        }

        public async Task<bool> UpdateAsync(int id, RuleName updated)
        {
            var existing = await _context.RuleNames.FindAsync(id);
            if (existing == null) return false;

            existing.Name = updated.Name;
            existing.Description = updated.Description;
            existing.Json = updated.Json;
            existing.Template = updated.Template;
            existing.SqlStr = updated.SqlStr;
            existing.SqlPart = updated.SqlPart;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var rule = await _context.RuleNames.FindAsync(id);
            if (rule == null) return false;

            _context.RuleNames.Remove(rule);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
