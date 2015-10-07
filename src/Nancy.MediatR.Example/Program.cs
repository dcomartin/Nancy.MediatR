using System;
using MediatR;
using Microsoft.Owin.Hosting;
using Owin;

namespace Nancy.MediatR.Example
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            using (WebApp.Start<Startup>("http://+:8888"))
            {
                Console.WriteLine("Running on http://localhost:8888");
                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
            }
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy(options =>
            {
                options.Bootstrapper = new Bootstrapper();
            });
        }
    }

    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(Nancy.TinyIoc.TinyIoCContainer container)
        {
            container.Register<InMemoryDatabase>().AsSingleton();
            container.Register<IMediator>(new Mediator(container.Resolve, container.ResolveAll));
            container.Register<IAsyncRequestHandler<SellInventory, Unit>, SellInventoryHandler>();
            container.Register<IAsyncRequestHandler<GetInventoryItem, InventoryItem>, GetInventoryItemHandler>();
        }
    }
}
