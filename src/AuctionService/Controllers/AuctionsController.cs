using AuctionService.Data;
using AuctionService.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionsController : ControllerBase
{
    private readonly AuctionDbContext _context;
    private readonly IMapper _mapper;

    public AuctionsController(AuctionDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions()
    {
        var auctions = await _context.Auctions
            .Include(a => a.Item)
            .OrderBy(a => a.Id)
            .ToListAsync();

        return _mapper.Map<List<AuctionDto>>(auctions);
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

    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction([FromBody] CreateAuctionDto auctionDto)
    {
        var auction = _mapper.Map<Auction>(auctionDto); // map the AuctionDto into an Auction entity
        // TODO: add current user as seller
        auction.Seller = "test";

        _context.Auctions.Add(auction);
        // this is just getting saved in the memory but not in the database and entity framework is tracking this because it's an entity. 

        bool result = await _context.SaveChangesAsync() > 0;

        if (!result)
            return BadRequest("Oops!! Could not create the auction. Please try again.");

        return CreatedAtAction(
            nameof(GetAuctionById),
            new { auction.Id },
            _mapper.Map<AuctionDto>(auction)
        );
    }


    [HttpPatch("{id}")]
    public async Task<ActionResult> UpdateAuction(Guid id, UpdateAutionDto updateAutionDto)
    {
        var auction = await _context.Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (auction == null) return NotFound();

        // Todo : check seller == user
        // TODO: If there are already some bids, then don't update. Return error

        auction.Item.Make = updateAutionDto.Make ?? auction.Item.Make;
        auction.Item.Model = updateAutionDto.Model ?? auction.Item.Model;
        auction.Item.Color = updateAutionDto.Color ?? auction.Item.Color;
        auction.Item.Year = updateAutionDto.Year ?? auction.Item.Year;
        auction.Item.Mileage = updateAutionDto.Mileage ?? auction.Item.Mileage;

        var result = await _context.SaveChangesAsync() > 0;

        if (result)
            return Ok("Auction updated successfully");

        return BadRequest("Oops!! Could not update the auction. Please try again.");
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuction(Guid id)
    {
        var auction = await _context.Auctions
            .FindAsync(id);

        if (auction == null)
            return NotFound();


        // TODO: Check Seller == username
        // ToDO: If there are already some bids, then don't delete. Return error

        _context.Auctions.Remove(auction);
        var res = _context.SaveChanges() > 0;

        if (!res)
            return BadRequest("Couldn't update DB");

        return Ok("Auction deleted successfully");
    }
}
