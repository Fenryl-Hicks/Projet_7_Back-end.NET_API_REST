using System;

namespace P7CreateRestApi.Entities
{
    public class Bid
    {
        int BidListId;
        string Account;
        string BidType;
        double? BidQuantity;
        double? AskQuantity;
        double? Ask;
        string Benchmark;
        DateTime? BidListDate;
        string Commentary;
        string BidSecurity;
        string BidStatus;
        string Trader;
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