using System;
using System.Threading.Tasks;

namespace Marketplace.Framework
{
    public static class ApplicationServiceExtensions
    {
        public static async Task HandleUpdateAsync<T, TId>(this IApplicationService service, IAggregateStore store, TId id, Action<T> action)
            where T : AggregateRoot<TId> where TId : Value<TId>
        {
            var aggregate = await store.LoadAsync<T, TId>(id);
            if (aggregate == null)
            {
                throw new InvalidOperationException($"Entity with id {id} cannot be found");
            }

            action(aggregate);
            await store.SaveAsync<T, TId>(aggregate);
        }
    }
}