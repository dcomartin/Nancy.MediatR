# Nancy.MediatR
Extending NancyFX to support a MediatR In-Process Request/Response.

# Purpose
The purpose of this extension is to allow you to generate a MediatR message, build a
pre and post handler pipeline for dealing with such issues as validation and permissions
and then passing the message to MediatR.

# Notes
Any message that has a response other than Unit, is made as a GET request.  Any other message
without a response is set as PUT request and will return a 204s status code.

# To Do
Add Hook for handling exceptions thrown from from Request Handler
Add hook for specifying response

# Example
```
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
     }
}
```
