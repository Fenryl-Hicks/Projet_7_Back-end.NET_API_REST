using System;

namespace P7CreateRestApi.Entities
{
    public class Trade
    {
        int TradeId;
        string Account;
        string AccountType;
        double? BuyQuantity;
        double? SellQuantity;
        double? BuyPrice;
        double? SellPrice;
        DateTime? TradeDate;
        string TradeSecurity;
        string TradeStatus;
        string Trader;
        string Benchmark;
        string Book;
        string CreationName;
        DateTime? CreationDate;
        string RevisionName;
        DateTime? RevisionDate;
        string DealName;
        string DealType;
        string SourceListId;
        string Side;
    }
}