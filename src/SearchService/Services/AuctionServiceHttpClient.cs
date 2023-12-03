using Microsoft.AspNetCore.Http.Features;
using MongoDB.Entities;

namespace SearchService;

public class AuctionServiceHttpClient {

    private readonly HttpClient _client;
    private readonly IConfiguration _config;
    public AuctionServiceHttpClient(HttpClient client, IConfiguration config) {
        _client = client;
        _config = config;
    }

    public async Task<List<Item>> GetItemsForSearchDb() {
        var lastUpdated = await DB.Find<Item, string>()
            .Sort(item => item.Descending(item => item.UpdatedAt))
            .Project(item => item.UpdatedAt.ToString())
            .ExecuteFirstAsync();

        return await _client.GetFromJsonAsync<List<Item>>(_config["AuctionServiceUrl"] + "/api/auctions?date=" + lastUpdated);
    }
}
