using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Nancy.MediatR.Tests
{
    public class TestModule : NancyModule
    {
        public TestModule(IMediator mediator)
        {
            this.SetMediatR(mediator);

            this.ForRequest<TestMessage>("/testmessage")
                .Bind(o => new TestMessage());

            this.ForRequest<TestMessageWithResponse, string>("/testmessagewithresponse")
                .Bind(o => new TestMessageWithResponse());
        }
    }

    public class TestMessage : IAsyncRequest { }

    public class TestMessageHandler : IAsyncRequestHandler<TestMessage, Unit>
    {
        public async Task<Unit> Handle(TestMessage message)
        {
            return await Task.FromResult(Unit.Value);
        }
    }

    public class TestMessageWithResponse : IAsyncRequest<string> { }

    public class TestMessageWithResponseHandler : IAsyncRequestHandler<TestMessageWithResponse, string>
    {
        public async Task<string> Handle(TestMessageWithResponse message)
        {
            return await Task.FromResult("Testing");
        }
    }
}
