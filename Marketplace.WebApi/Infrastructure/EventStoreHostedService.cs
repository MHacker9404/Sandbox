using System.Threading;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Microsoft.Extensions.Hosting;

namespace Marketplace.WebApi.Infrastructure
{
    public class EventStoreHostedService : IHostedService
    {
        private readonly IEventStoreConnection _eventStore;
        private readonly ProjectionManager _projectionManager;

        //public EventStoreHostedService(IEventStoreConnection eventStore, EaSubscription subscription)
        public EventStoreHostedService(IEventStoreConnection eventStore, ProjectionManager projectionManager)
        {
            _eventStore = eventStore;
            _projectionManager = projectionManager;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _eventStore.ConnectAsync();
            _projectionManager.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _projectionManager.Stop();
            _eventStore.Close();
            return Task.CompletedTask;
        }
    }
}