using System;
using AuctionService.DTOs;
using AutoMapper;

namespace AuctionService.RequestHelpers;

public class MappingsProfile : Profile
{
    public MappingsProfile()
    {
        CreateMap<Auction, AuctionDto>().IncludeMembers(a => a.Item);
        CreateMap<Item, AuctionDto>();
        CreateMap<CreateAuctionDto, Auction>()
            .ForMember(d => d.Item, o => o.MapFrom(s => s));
        CreateMap<CreateAuctionDto, Item>();
    }
}
