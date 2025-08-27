using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Data;
using P7CreateRestApi.Entities;

namespace P7CreateRestApi.Repositories
{
    public class TradeRepository : ITradeRepository
    {
        private readonly LocalDbContext _context;

        public TradeRepository(LocalDbContext context)
        {
            _context = context;
        }

        public async Task<List<Trade>> GetAllAsync()
        {
            return await _context.Trades.ToListAsync();
        }

        public async Task<Trade?> GetByIdAsync(int id)
        {
            return await _context.Trades.FindAsync(id);
        }

        public async Task<Trade> AddAsync(Trade trade)
        {
            _context.Trades.Add(trade);
            await _context.SaveChangesAsync();
            return trade;
        }

        public async Task<bool> UpdateAsync(int id, Trade updated)
        {
            var existing = await _context.Trades.FindAsync(id);
            if (existing == null) return false;

            existing.Account = updated.Account;
            existing.AccountType = updated.AccountType;
            existing.BuyQuantity = updated.BuyQuantity;
            existing.SellQuantity = updated.SellQuantity;
            existing.BuyPrice = updated.BuyPrice;
            existing.SellPrice = updated.SellPrice;
            existing.TradeDate = updated.TradeDate;
            existing.TradeSecurity = updated.TradeSecurity;
            existing.TradeStatus = updated.TradeStatus;
            existing.Trader = updated.Trader;
            existing.Benchmark = updated.Benchmark;
            existing.Book = updated.Book;
            existing.CreationName = updated.CreationName;
            existing.CreationDate = updated.CreationDate;
            existing.RevisionName = updated.RevisionName;
            existing.RevisionDate = updated.RevisionDate;
            existing.DealName = updated.DealName;
            existing.DealType = updated.DealType;
            existing.SourceListId = updated.SourceListId;
            existing.Side = updated.Side;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var trade = await _context.Trades.FindAsync(id);
            if (trade == null) return false;

            _context.Trades.Remove(trade);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
