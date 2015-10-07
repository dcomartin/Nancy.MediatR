using System;
using MediatR;
using Nancy.ModelBinding;

namespace Nancy.MediatR.Example
{
    public class InventoryModule : NancyModule
    {
        public InventoryModule(IMediator mediator)
        {
            this.SetMediatR(mediator);
            
            this.ForRequest<SellInventory>("/inventory/{InventoryId:int}/sell")
                .Bind(parameters =>
                {
                    // Bind using standard binding, but manually set the InventoryId based on parameter
                    var message = this.Bind<SellInventory>();
                    message.InventoryId = (int) parameters.InventoryId;
                    return message;
                })
                .Pre(next => (async (message, parameters, ct) =>
                {
                    // Possibly check permissions?
                    Console.WriteLine("SellInventory Pre #1");

                    // Call next Pre
                    await next(message, parameters, ct);
                }))
                .Pre(next => (async (message, parameters, ct) =>
                {
                    // Possibly validate request?
                    Console.WriteLine("SellInventory Pre #2");

                    await next(message, parameters, ct);
                }));

            this.ForRequest<GetInventoryItem, InventoryItem>("/inventory/{InventoryId:int}")
                .Bind(parameters => new GetInventoryItem((int) parameters.InventoryId))
                .Pre(next => (async (message, parameters, ct) =>
                {
                    // Possibly check permissions?
                    Console.WriteLine("GetInventory Pre #1");

                    // Call next Pre
                    await next(message, parameters, ct);
                }))
                .Pre(next => (async (message, parameters, ct) =>
                {
                    // Possibly validate request?
                    Console.WriteLine("GetInventory Pre #2");

                    await next(message, parameters, ct);
                }))
                .Post(next => (async (message, result, ct) =>
                {
                    // Possibly log output?
                    Console.WriteLine("GetInventory Post #1");

                    // Call next Post
                    return await next(message, result, ct);
                }))
                .Post(next => (async (message, result, ct) =>
                {
                    // Possibly override output data?
                    Console.WriteLine("GetInventory Post #2");

                    // Call next Post
                    return await next(message, result, ct);
                }));
        }
    }
}