using System.Threading.Tasks;

namespace Marketplace.Framework
{
    public interface IAggregateStore
    {
        Task<bool> ExistsAsync<T, TId>(TId aggregateId) where T : AggregateRoot<TId> where TId : Value<TId>;
        Task SaveAsync<T, TId>(T aggregate) where T : AggregateRoot<TId> where TId : Value<TId>;
        Task<T> LoadAsync<T, TId>(TId aggregateId) where T : AggregateRoot<TId> where TId : Value<TId>;
    }
}