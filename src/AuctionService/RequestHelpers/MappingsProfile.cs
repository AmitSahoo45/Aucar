using System;
using AuctionService.DTOs;
using AutoMapper;
using Contracts;

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
        CreateMap<AuctionDto, AuctionCreated>(); // This auction dto can't be known what it is by the search service. 
        // so we need to have something in between and that in between is AuctionCreated -> the contract. both the search service and the auction service knows about this. 
        
    }
}
