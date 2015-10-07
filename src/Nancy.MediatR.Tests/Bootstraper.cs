using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Nancy.MediatR.Tests
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(Nancy.TinyIoc.TinyIoCContainer container)
        {
            container.Register<IMediator>(new Mediator(container.Resolve, container.ResolveAll));
            container.Register<IAsyncRequestHandler<TestMessage, Unit>, TestMessageHandler>();
            container.Register<IAsyncRequestHandler<TestMessageWithResponse, string>, TestMessageWithResponseHandler>();
        }
    }
}
