using P7CreateRestApi.Data;
using P7CreateRestApi.Entities;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Repositories.Interfaces;

namespace P7CreateRestApi.Repositories
{
    public class CurvePointRepository : ICurvePointRepository
    {
        private readonly LocalDbContext _context;

        public CurvePointRepository(LocalDbContext context)
        {
            _context = context;
        }

        public async Task<List<CurvePoint>> GetAllAsync()
        {
            return await _context.CurvePoints.ToListAsync();
        }

        public async Task<CurvePoint?> GetByIdAsync(int id)
        {
            return await _context.CurvePoints.FindAsync(id);
        }

        public async Task<CurvePoint> AddAsync(CurvePoint curve)
        {
            _context.CurvePoints.Add(curve);
            await _context.SaveChangesAsync();
            return curve;
        }

        public async Task<bool> UpdateAsync(int id, CurvePoint updated)
        {
            var existing = await _context.CurvePoints.FindAsync(id);
            if (existing == null) return false;

            existing.CurveId = updated.CurveId;
            existing.AsOfDate = updated.AsOfDate;
            existing.Term = updated.Term;
            existing.CurvePointValue = updated.CurvePointValue;
            existing.CreationDate = updated.CreationDate;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var curve = await _context.CurvePoints.FindAsync(id);
            if (curve == null) return false;

            _context.CurvePoints.Remove(curve);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
