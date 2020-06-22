namespace COR.FirstLook.Handlers
{
    public abstract class Handler<T> : IHandler<T> where T : class
    {
        private IHandler<T> Next { get; set; }

        public IHandler<T> SetNext(IHandler<T> next)
        {
            Next = next;
            return Next;
        }

        public virtual void Handle(T request) => Next?.Handle(request);
    }
}