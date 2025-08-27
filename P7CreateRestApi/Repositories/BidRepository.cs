using P7CreateRestApi.Entities;
using P7CreateRestApi.Data;
using Microsoft.EntityFrameworkCore;

namespace P7CreateRestApi.Repositories
{
    public class BidRepository : IBidRepository
    {
        private readonly LocalDbContext _context;

        public BidRepository(LocalDbContext context)
        {
            _context = context;
        }

        public async Task<List<Bid>> GetAllAsync()
        {
            return await _context.Bids.ToListAsync();
        }

        public async Task<Bid?> GetByIdAsync(int id)
        {
            return await _context.Bids.FindAsync(id);
        }

        public async Task<Bid> AddAsync(Bid bid)
        {
            _context.Bids.Add(bid);
            await _context.SaveChangesAsync();
            return bid;
        }

        public async Task<bool> UpdateAsync(int id, Bid bid)
        {
            var existing = await _context.Bids.FindAsync(id);
            if (existing == null) return false;

            existing.Account = bid.Account;
            existing.BidType = bid.BidType;
            existing.BidQuantity = bid.BidQuantity;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var bid = await _context.Bids.FindAsync(id);
            if (bid == null) return false;

            _context.Bids.Remove(bid);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
