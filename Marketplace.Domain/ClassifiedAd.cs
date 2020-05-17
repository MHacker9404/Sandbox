using Marketplace.Domain.Events;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class ClassifiedAd : Entity
    {
        protected  virtual void EnsureValidState()
        {
            var valid = Id != null && OwnerId != null && (State switch
                                                          {
                                                              ClassifiedAdState.PendingReview => Title != null && Text != null && Price?.Amount > 0
                                                              , ClassifiedAdState.Active => Title != null && Text != null && Price?.Amount > 0
                                                                                            && ApprovedBy != null
                                                              , _ => true
                                                          });
            if (!valid)
            {
                throw new InvalidEntityStateException(this, $"Post-checks failed in state {State}");
            }
        }

        public ClassifiedAd(ClassifiedAdId id, UserId ownerId)
        {
            Id = id;
            OwnerId = ownerId;
            State = ClassifiedAdState.Inactive;
            EnsureValidState();

            Raise(new ClassifiedAdCreated {Id = id.Value, OwnerId = ownerId.Value});
        }

        public ClassifiedAdId Id { get; }
        public UserId OwnerId { get; }
        public ClassifiedAdTitle Title { get; private set; }
        public ClassifiedAdText Text { get; private set; }
        public Price Price { get; private set; }

        public void SetTitle(ClassifiedAdTitle title)
        {
            Title = title;
            EnsureValidState();

            Raise(new ClassifiedAdTitleChanged {Id = Id, Title = Title});
        }

        public void UpdateText(ClassifiedAdText text)
        {
            Text = text;
            EnsureValidState();

            Raise(new ClassifiedAdTextUpdated {Id = Id, Text = Text});
        }

        public void UpdatePrice(Price price)
        {
            Price = price;
            EnsureValidState();

            Raise(new ClassifiedAdPriceUpdated {Id = Id, Price = Price, Currency = Price.Currency});
        }

        public void RequestToPublish()
        {
            State = ClassifiedAdState.PendingReview;
            EnsureValidState();

            Raise(new ClassifiedAdSentForReview {Id = Id});
        }

        public ClassifiedAdState State { get; private set; }
        public UserId ApprovedBy { get; private set; }

        public enum ClassifiedAdState
        {
            PendingReview
            , Active
            , Inactive
            , Sold
        }
    }
}