﻿using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using ZstdSharp.Unsafe;

namespace SearchService;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase {
    [HttpGet]
    public async Task<ActionResult<List<Item>>> SearchItems([FromQuery]SearchParams searchParams){
        var query = DB.PagedSearch<Item, Item>();

        query.Sort(item => item.Ascending(item => item.Make));

        if(!string.IsNullOrEmpty(searchParams.SearchTerm)) {
            query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
        }

        query = searchParams.OrderBy switch {
            "make" => query.Sort(item => item.Ascending(item => item.Make)),
            "new" => query.Sort(item => item.Descending(item => item.CreatedAt)),
            _ => query.Sort(item => item.Ascending(item => item.AuctionEnd))
        };

        query = searchParams.FilterBy switch {
            "finished" => query.Match(item => item.AuctionEnd < DateTime.UtcNow),
            "endingSoon" => query.Match(
                item => item.AuctionEnd < DateTime.UtcNow.AddHours(6) && item.AuctionEnd > DateTime.UtcNow),
            _ => query.Match(item => item.AuctionEnd > DateTime.UtcNow)
        };

        if(!string.IsNullOrEmpty(searchParams.Seller)) {
            query.Match(item => item.Seller == searchParams.Seller);
        }

        if(!string.IsNullOrEmpty(searchParams.Winnner)) {
            query.Match(item => item.Winner == searchParams.Winnner);
        }

        query.PageNumber(searchParams.PageNumber);
        query.PageSize(searchParams.PageSize);

        var result = await query.ExecuteAsync();

        return Ok(new {
            results = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount
        });
    }
}
