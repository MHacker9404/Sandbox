using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Marketplace.Framework;
using Newtonsoft.Json;

namespace Marketplace.WebApi.Infrastructure
{
    public class EsAggregateStore : IAggregateStore
    {
        private readonly IEventStoreConnection _eventStore;

        public EsAggregateStore(IEventStoreConnection eventStore) => _eventStore = eventStore;

        public async Task<bool> ExistsAsync<T, TId>(TId aggregateId) where T : AggregateRoot<TId> where TId : Value<TId>
        {
            var stream = GetStreamName<T, TId>(aggregateId);
            var result = await _eventStore.ReadEventAsync(stream, 1, false);
            return result.Status != EventReadStatus.NoStream;
        }

        public async Task SaveAsync<T, TId>(T aggregate) where T : AggregateRoot<TId> where TId : Value<TId>
        {
            if (aggregate == null) throw new ArgumentNullException(nameof(aggregate));

            var changes = aggregate.GetChanges().Select(evt => new EventData(Guid.NewGuid()
                                                                             , evt.GetType().Name
                                                                             , true
                                                                             , Serialize(evt)
                                                                             , Serialize(new EventMetadata
                                                                                         {
                                                                                             ClrType = evt.GetType().AssemblyQualifiedName
                                                                                         }))).ToArray();
            if (!changes.Any()) return;
            var streamName = GetStreamName<T, TId>(aggregate);
            await _eventStore.AppendToStreamAsync(streamName, aggregate.Version, changes);
            aggregate.ClearChanges();
        }

        public async Task<T> LoadAsync<T, TId>(TId aggregateId) where T : AggregateRoot<TId> where TId : Value<TId>
        {
            if (aggregateId == null) throw new ArgumentNullException(nameof(aggregateId));

            var stream = GetStreamName<T, TId>(aggregateId);
            var aggregate = (T) Activator.CreateInstance(typeof(T), true);

            var page = await _eventStore.ReadStreamEventsForwardAsync(stream, 0, 1024, false);
            aggregate.Load(page.Events.Select(evt => evt.Deserialize()).ToArray());
            return aggregate;
        }

        private static string GetStreamName<T, TId>(TId aggregrateId) where T : AggregateRoot<TId> where TId : Value<TId> =>
            $"{typeof(T).Name}-{aggregrateId}";

        private static string GetStreamName<T, TId>(T aggregate) where T : AggregateRoot<TId> where TId : Value<TId> =>
            $"{typeof(T).Name}-{aggregate.Id}";

        private static byte[] Serialize(object data) => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
    }
}