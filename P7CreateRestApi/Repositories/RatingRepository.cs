using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Data;
using P7CreateRestApi.Entities;
using P7CreateRestApi.Repositories.Interfaces;


namespace P7CreateRestApi.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly LocalDbContext _context;

        public RatingRepository(LocalDbContext context)
        {
            _context = context;
        }

        public async Task<List<Rating>> GetAllAsync()
        {
            return await _context.Ratings.ToListAsync();
        }

        public async Task<Rating?> GetByIdAsync(int id)
        {
            return await _context.Ratings.FindAsync(id);
        }

        public async Task<Rating> AddAsync(Rating rating)
        {
            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();
            return rating;
        }

        public async Task<bool> UpdateAsync(int id, Rating updated)
        {
            var existing = await _context.Ratings.FindAsync(id);
            if (existing == null) return false;

            existing.MoodysRating = updated.MoodysRating;
            existing.SandPRating = updated.SandPRating;
            existing.FitchRating = updated.FitchRating;
            existing.OrderNumber = updated.OrderNumber;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var rating = await _context.Ratings.FindAsync(id);
            if (rating == null) return false;

            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
