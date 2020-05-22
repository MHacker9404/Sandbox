using System.Threading;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Microsoft.Extensions.Hosting;

namespace Marketplace.WebApi.Infrastructure
{
    public class EventStoreHostedService : IHostedService
    {
        private readonly IEventStoreConnection _eventStore;

        public EventStoreHostedService(IEventStoreConnection eventStore) => _eventStore = eventStore;
        public Task StartAsync(CancellationToken cancellationToken) => _eventStore.ConnectAsync();

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _eventStore.Close();
            return Task.CompletedTask;
        }
    }
}