using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionsController : ControllerBase
{
    private readonly AuctionDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public AuctionsController(AuctionDbContext context, IMapper mapper,
    IPublishEndpoint publishEndpoint) // ipublish will help us to publish that message. 
    {
        _context = context;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions(string? date)
    {
        var query = _context.Auctions.OrderBy(x => x.Item.Make).AsQueryable();

        if (!string.IsNullOrEmpty(date))
            query = query.Where(x => x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);

        return await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
    {
        var auction = await _context.Auctions
            .Include(a => a.Item)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (auction == null)
            return NotFound();

        return _mapper.Map<AuctionDto>(auction);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto createAuctionDto)
    {
        var auction = _mapper.Map<Auction>(createAuctionDto); // map the AuctionDto into an Auction entity
        auction.Seller = User.Identity.Name; // setting the username to the seller property of the auction entity

        _context.Auctions.Add(auction);
        // this is just getting saved in the memory but not in the database and entity framework is tracking this because it's an entity. 
        var newAuction = _mapper.Map<AuctionDto>(auction);
        // publish the message
        await _publishEndpoint.Publish(_mapper.Map<AuctionCreated>(newAuction));
        // after the auction is created a message needs to be published. 
        // in order to publish the created auction, we need to map it to an auction created object. -> this is done in mapping profiles.cs of this project


        bool result = await _context.SaveChangesAsync() > 0;

        if (!result)
            return BadRequest("Oops!! Could not create the auction. Please try again.");

        return CreatedAtAction(
            nameof(GetAuctionById),
            new { auction.Id },
            newAuction
        );
    }

    [Authorize]
    [HttpPatch("{id}")]
    public async Task<ActionResult> UpdateAuction(Guid id, UpdateAutionDto updateAutionDto)
    {
        var auction = await _context.Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (auction == null) return NotFound();

        if (auction.Seller != User.Identity.Name)
            return Forbid();
        // TODO: If there are already some bids, then don't update. Return error

        auction.Item.Make = updateAutionDto.Make ?? auction.Item.Make;
        auction.Item.Model = updateAutionDto.Model ?? auction.Item.Model;
        auction.Item.Color = updateAutionDto.Color ?? auction.Item.Color;
        auction.Item.Year = updateAutionDto.Year ?? auction.Item.Year;
        auction.Item.Mileage = updateAutionDto.Mileage ?? auction.Item.Mileage;

        await _publishEndpoint.Publish(_mapper.Map<AuctionUpdated>(auction));

        var result = await _context.SaveChangesAsync() > 0;

        if (result)
            return Ok("Auction updated successfully");

        return BadRequest("Oops!! Could not update the auction. Please try again.");
    }

[Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuction(Guid id)
    {
        var auction = await _context.Auctions
            .FindAsync(id);

        if (auction == null)
            return NotFound();

        // TODO: Check Seller == username
        if (auction.Seller != User.Identity.Name)
            return Forbid();
        // ToDO: If there are already some bids, then don't delete. Return error

        await _publishEndpoint.Publish<AuctionDeleted>(new { Id = auction.Id.ToString() });

        _context.Auctions.Remove(auction);
        var res = _context.SaveChanges() > 0;

        if (!res)
            return BadRequest("Couldn't update DB");

        return Ok("Auction deleted successfully");
    }
}
