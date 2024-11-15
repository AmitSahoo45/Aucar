using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.Services;

namespace SearchService.Data;

public class DbInitializer
{
    public static async Task InitDB(WebApplication app)
    {
        await DB.InitAsync("SearchDb", MongoClientSettings.FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

        await DB.Index<Item>()
                .Key(x => x.Make, KeyType.Text)
                .Key(x => x.Model, KeyType.Text)
                .Key(x => x.Color, KeyType.Text)
                .CreateAsync();

        var count = await DB.CountAsync<Item>();

        // if (count == 0)
        // {
        //     Console.WriteLine("Seeding data...");
        //     var itemData = await File.ReadAllTextAsync("Data/auctions.json");
        //     var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        //     var items = JsonSerializer.Deserialize<List<Item>>(itemData, options);
        //     await DB.SaveAsync(items);
        // }

        using var scope = app.Services.CreateScope();
        var httpClient = scope.ServiceProvider.GetRequiredService<AuctionSvcHttpClient>();
        var items = await httpClient.GetItemsForSearchDb();

        Console.WriteLine("Seeding data...");
        Console.WriteLine(items.Count + " returned from the Auction Service");

        if (items.Count > 0)
        {
            foreach (var item in items)
            {
                Console.WriteLine(item.Make + " " + item.Model);
            }
            await DB.SaveAsync(items);
        }
    }
}
