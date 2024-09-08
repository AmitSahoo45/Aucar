using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.RequestHelpers;

namespace SearchService.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Item>>> SearchItems([FromQuery] SearchParams sp)
    {
        var query = DB.PagedSearch<Item, Item>(); // Item, Item coz ordering at Orderby "make" was throwing error 

        if (!string.IsNullOrEmpty(sp.searchItem))
            query.Match(Search.Full, sp.searchItem).SortByTextScore();

        query = sp.OrderBy switch
        {
            "make" => query.Sort(x => x.Ascending(a => a.Make)), // sorted by alphabetical order
            "new" => query.Sort(x => x.Descending(a => a.CreatedAt)), // ordered added more recently
            _ => query.Sort(x => x.Ascending(a => a.AuctionEnd)) // Default sorting
        };

        query = sp.FilterBy switch
        {
            "finished" => query.Match(x => x.AuctionEnd < DateTime.UtcNow), // Auctions that have ended
            "endingSoon" => query.Match(x => x.AuctionEnd > DateTime.UtcNow.AddHours(6) && x.AuctionEnd > DateTime.UtcNow), // Auctions that are ending soon
            _ => query.Match(x => x.AuctionEnd > DateTime.UtcNow) // Default filter
        };

        if (!string.IsNullOrEmpty(sp.Seller))
            query.Match(x => x.Seller == sp.Seller);

        if (!string.IsNullOrEmpty(sp.Winner)) 
            query.Match(x => x.Winner == sp.Winner);

        query.PageNumber(sp.pageNumber)
            .PageSize(sp.pageSize);

        var res = await query.ExecuteAsync();

        return Ok(new
        {
            res = res.Results,
            pageCount = res.PageCount,
            totalCount = res.TotalCount
        });
    }
}
