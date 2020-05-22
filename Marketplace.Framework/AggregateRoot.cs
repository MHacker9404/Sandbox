using System.Collections.Generic;
using System.Linq;

namespace Marketplace.Framework
{
    public abstract class AggregateRoot<TId> : IInternalEventHandler where TId : Value<TId>
    {
        private readonly List<object> _changes;

        protected AggregateRoot() => _changes = new List<object>();
        public TId Id { get; protected set; }

        public long Version { get; private set; } = -1;

        void IInternalEventHandler.Handle(object @event) => When(@event);

        protected void Apply(object @event)
        {
            When(@event);
            EnsureValidState();
            _changes.Add(@event);
        }

        protected void ApplyToEntity(IInternalEventHandler entity, object @event) => entity?.Handle(@event);

        protected abstract void EnsureValidState();

        protected abstract void When(object @event);

        public IEnumerable<object> GetChanges() => _changes.AsEnumerable();
        public void ClearChanges() => _changes.Clear();

        public void Load(IEnumerable<object> history)
        {
            foreach (var evt in history)
            {
                When(evt);
                Version++;
            }
        }
    }
}