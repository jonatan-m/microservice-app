using Contracts;
using MassTransit;

namespace AuctionService;

public class AuctionCreatedFaultConsumer : IConsumer<Fault<AuctionCreated>> {
    public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context) {

        Console.WriteLine("--> Consuming faulty auction creation");

        var exception = context.Message.Exceptions.First();

        if(exception.ExceptionType == "System.ArgumentException"){
            context.Message.Message.Model = "Fiesta";
            await context.Publish(context.Message.Message);
        }
        else {
            Console.WriteLine("Im not dealing with this type of exception");
        }
    }
}
