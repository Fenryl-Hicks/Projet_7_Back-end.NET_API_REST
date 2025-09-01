using AutoMapper;
using P7CreateRestApi.Entities;
using P7CreateRestApi.Dtos.Bids;
using P7CreateRestApi.Dtos.Curves;
using P7CreateRestApi.Dtos.Ratings;
using P7CreateRestApi.Dtos.Trades;
using P7CreateRestApi.Dtos.RuleNames;

namespace P7CreateRestApi.Mapping
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            // Bid
            CreateMap<CreateBidRequestDto, Bid>();
            CreateMap<UpdateBidRequestDto, Bid>();
            CreateMap<Bid, BidResponseDto>();
            CreateMap<Bid, BidListItemDto>();

            // CurvePoint
            CreateMap<CreateCurvePointRequestDto, CurvePoint>();
            CreateMap<UpdateCurvePointRequestDto, CurvePoint>();
            CreateMap<CurvePoint, CurvePointResponseDto>();
            CreateMap<CurvePoint, CurvePointListItemDto>();

            // Rating
            CreateMap<CreateRatingRequestDto, Rating>();
            CreateMap<UpdateRatingRequestDto, Rating>();
            CreateMap<Rating, RatingResponseDto>();
            CreateMap<Rating, RatingListItemDto>();

            // RuleName
            CreateMap<CreateRuleNameRequestDto, RuleName>();
            CreateMap<UpdateRuleNameRequestDto, RuleName>();
            CreateMap<RuleName, RuleNameResponseDto>();
            CreateMap<RuleName, RuleNameListItemDto>();


            // Trade
            CreateMap<CreateTradeRequestDto, Trade>();
            CreateMap<UpdateTradeRequestDto, Trade>();
            CreateMap<Trade, TradeResponseDto>();
            CreateMap<Trade, TradeListItemDto>();
        }
    }
}
