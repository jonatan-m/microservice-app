using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace SearchService;

public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated> {
    public async Task Consume(ConsumeContext<AuctionUpdated> context) {
        Console.WriteLine("--> Consuming auction updated");

        var id = context.Message.Id;

        await DB.Update<Item>()
            .MatchID(id)
            .Modify(item => item.Make, context.Message.Make)
            .Modify(item => item.Model, context.Message.Model)
            .Modify(item => item.Year, context.Message.Year)
            .Modify(item => item.Color, context.Message.Color)
            .Modify(item => item.Mileage, context.Message.Mileage)
            .ExecuteAsync(); 
    }
}
