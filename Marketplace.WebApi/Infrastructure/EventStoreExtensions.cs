using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace Marketplace.WebApi.Infrastructure
{
    public static class EventStoreExtensions
    {
        public static Task AppendEvents(this IEventStoreConnection connection, string streamName, long version, params object[] events)
        {
            if (events == null || !events.Any())
                return Task.CompletedTask;

            var preparedEvents = events.Select(e =>
                                                   new EventData(Guid.NewGuid()
                                                                 , e.GetType().Name
                                                                 , true
                                                                 , Serialize(e)
                                                                 , Serialize(
                                                                     new EventMetadata {ClrType = e.GetType().AssemblyQualifiedName})
                                                   )).ToArray();

            return connection.AppendToStreamAsync(streamName, version, preparedEvents);
        }

        private static byte[] Serialize(object data) => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
    }
}