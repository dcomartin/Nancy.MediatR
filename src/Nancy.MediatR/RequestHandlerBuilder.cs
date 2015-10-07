using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Nancy.MediatR
{
    public delegate Task PreHandler<in TMessage>(TMessage message, dynamic parameters, CancellationToken ct);

    public delegate PreHandler<TMessage> PrePipe<TMessage, TResult>(PreHandler<TMessage> next)
        where TMessage : IAsyncRequest<TResult>;

    public delegate Task<TResult> PostHandler<in TMessage, TResult>(TMessage message, TResult result, CancellationToken ct)
       where TMessage : IAsyncRequest<TResult>;

    public delegate PostHandler<TMessage, TResult> PostPipe<TMessage, TResult>(PostHandler<TMessage, TResult> next)
        where TMessage : IAsyncRequest<TResult>;

    public class RequestHandlerBuilder<TCommand, TResponse>
        where TCommand : IAsyncRequest<TResponse>
    {
        private readonly IMediator _mediator;

        private readonly Stack<PrePipe<TCommand, TResponse>> _prePipes = new Stack<PrePipe<TCommand, TResponse>>();
        private readonly Stack<PostPipe<TCommand, TResponse>> _postPipes = new Stack<PostPipe<TCommand, TResponse>>();

        private Func<dynamic, TCommand> _buildCommandFunc;
        public dynamic Parameters { private get; set; }

        internal RequestHandlerBuilder(IMediator mediator)
        {
            _mediator = mediator;
        }

        public RequestHandlerBuilder<TCommand, TResponse> Pre(PrePipe<TCommand, TResponse> prePipe)
        {
            _prePipes.Push(prePipe);
            return this;
        }

        public RequestHandlerBuilder<TCommand, TResponse> Post(PostPipe<TCommand, TResponse> postPipe)
        {
            _postPipes.Push(postPipe);
            return this;
        }

        public RequestHandlerBuilder<TCommand, TResponse> Bind(Func<dynamic, TCommand> bindRequestFunc)
        {
            _buildCommandFunc = bindRequestFunc;
            return this;
        }

        public async Task<TResponse> Handle(CancellationToken cancellationToken)
        {
            var preHandler = new PreHandler<TCommand>(async (message, parameters, ct) => await Task.FromResult(0));

            while (_prePipes.Count > 0)
            {
                var pipe = _prePipes.Pop();
                preHandler = pipe(preHandler);
            }

            var command = _buildCommandFunc(Parameters);
            await preHandler(command, Parameters, cancellationToken);

            var cmdResult = await _mediator.SendAsync<TResponse>(command);

            var postHandler = new PostHandler<TCommand, TResponse>(async (message, response, ct) => await Task.FromResult(response));
            while (_postPipes.Count > 0)
            {
                var pipe = _postPipes.Pop();
                postHandler = pipe(postHandler);
            }

            var result = await postHandler(command, cmdResult, cancellationToken);
            return result;
        }
    }
}
