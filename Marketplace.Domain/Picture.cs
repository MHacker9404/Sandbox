using System;
using Marketplace.Domain.Events;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class Picture : Entity<PictureId>
    {
        public Picture(Action<object> applier) : base(applier) { }

        public PictureSize Size { get; private set; }
        public Uri Location { get; private set; }
        public int Order { get; private set; }

        //protected override void EnsureValidState()
        //{
        //    throw new NotImplementedException();
        //}

        protected override void When(object @event)
        {
            switch (@event)
            {
                case PictureAdded e:
                    Id = PictureId.FromGuid(e.Id);
                    Location = new Uri(e.Url);
                    Size = PictureSize.FromHeightWidth(e.Height, e.Width);
                    Order = e.Order;
                    break;

                case PictureResized e:
                    Size = PictureSize.FromHeightWidth(e.Height, e.Width);
                    break;
            }
        }

        public void Resize(PictureSize size) => Apply(new PictureResized {PictureId = Id.Value, Height = size.Height, Width = size.Width});
    }
}