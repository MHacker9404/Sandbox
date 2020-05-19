using System;
using System.Collections.Generic;

namespace Marketplace.Framework
{
    public abstract class Entity<TId> : IInternalEventHandler where TId : Value<TId>
    {
        private readonly Action<object> _applier;
        private readonly List<object> _events;

        protected Entity() => _events = new List<object>();

        protected Entity(Action<object> applier) : this() => _applier = applier;
        public TId Id { get; protected set; }

        //public IEnumerable<object> GetChanges() => _events.AsEnumerable();
        //public void ClearChanges() => _events.Clear();

        void IInternalEventHandler.Handle(object @event) => When(@event);

        protected void Apply(object @event)
        {
            When(@event);
            //EnsureValidState();
            //_events.Add(@event);
            _applier(@event);
        }

        //protected abstract void EnsureValidState();

        protected abstract void When(object @event);
    }
}