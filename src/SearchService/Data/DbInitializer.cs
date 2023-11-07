using System.Text.Json;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.VisualBasic;
using MongoDB.Driver;
using MongoDB.Entities;

namespace SearchService;

public class DbInitializer {
    public static async Task InitDb(WebApplication app){
        await DB.InitAsync("SearchDb", MongoClientSettings
            .FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

        await DB.Index<Item>()
            .Key(item => item.Make, KeyType.Text)
            .Key(item => item.Model, KeyType.Text)
            .Key(item => item.Color, KeyType.Text)
            .CreateAsync();

        var count = await DB.CountAsync<Item>();

        using var scope = app.Services.CreateScope();

        var httpClient = scope.ServiceProvider.GetRequiredService<AuctionServiceHttpClient>();

        var items = await httpClient.GetItemsForSearchDb();

        Console.WriteLine(items.Count + " returned from auction service");

        if (items.Count > 0) await DB.SaveAsync(items);
    } 

}
