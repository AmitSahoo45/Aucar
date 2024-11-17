using System;
using MassTransit;
using Contracts;
using AutoMapper;
using SearchService.Models;
using MongoDB.Entities;

namespace SearchService.Consumers;

public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
{
    private readonly IMapper _mapper;

    // now that we know we are using auto mapper we can inject it into this class 
    public AuctionCreatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }

    // the auction created needs to be mapped suucessfully into an item so we can update our database
    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        Console.WriteLine("--> Consuming Auction Created: " + context.Message.Id);
        var item = _mapper.Map<Item>(context.Message);

        if (item.Model == "Foo")
            throw new ArgumentException("Cannot sell cars with name of Foo");
            
        await item.SaveAsync();
    }
}
