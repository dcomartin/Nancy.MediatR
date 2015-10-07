using System;
using MediatR;

namespace Nancy.MediatR
{
    public static class NancyMediatRModule
    {
        private static IMediator _mediator;

        public static void SetMediatR(this NancyModule module, IMediator mediator)
        {
            _mediator = mediator;
        }

        public static RequestHandlerBuilder<TMessage, TResponse> ForRequest<TMessage, TResponse>(this NancyModule module, string path)
            where TMessage : IAsyncRequest<TResponse>
        {
            if (_mediator == null)
            {
                throw new InvalidOperationException($"Mediator has not been assigned.  SetMediatR(IMediator) must be called prior to calling ForRequest().");
            }

            var cmdBuilder = new RequestHandlerBuilder<TMessage, TResponse>(_mediator);

            module.Get[path, true] = async (o, cancellationToken) =>
            {
                cmdBuilder.Parameters = o;
                return await cmdBuilder.Handle(cancellationToken);
            };

            return cmdBuilder;
        }

        public static RequestHandlerBuilder<TMessage, Unit> ForRequest<TMessage>(this NancyModule module, string path)
            where TMessage : IAsyncRequest
        {
            if (_mediator == null)
            {
                throw new InvalidOperationException($"Mediator has not been assigned.  SetMediatR(IMediator) must be called prior to calling ForRequest().");
            }

            var cmdBuilder = new RequestHandlerBuilder<TMessage, Unit>(_mediator);

            module.Put[path, true] = async (o, cancellationToken) =>
            {
                cmdBuilder.Parameters = o;
                await cmdBuilder.Handle(cancellationToken);
                return HttpStatusCode.NoContent;
            };

            return cmdBuilder;
        }
    }   
}