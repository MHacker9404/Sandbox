namespace MediatorDemo {
    public abstract class Colleague
    {
        protected Mediator _mediator;

        //public Colleague(Mediator mediator)
        //{
        //    _mediator = mediator;
        //}

        internal void SetMediator(Mediator mediator)
        {
            this._mediator = mediator;
        }

        public virtual void Send(string message)
        {
            this._mediator.Send(message, this);
        }

        public abstract void HandleNotification(string message);
    }
}