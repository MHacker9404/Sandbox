using System;
using Marketplace.Domain.Events;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class ClassifiedAd : Entity
    {
        public enum ClassifiedAdState
        {
            PendingReview
            , Active
            , Inactive
            , Sold
        }

        public ClassifiedAd(ClassifiedAdId id, UserId ownerId) => Apply(new ClassifiedAdCreated {Id = id, OwnerId = ownerId});

        public ClassifiedAdId Id { get; private set; }
        public UserId OwnerId { get; private set; }
        public ClassifiedAdTitle Title { get; private set; }
        public ClassifiedAdText Text { get; private set; }
        public Price Price { get; private set; }

        public ClassifiedAdState State { get; private set; }
        public UserId ApprovedBy { get; private set; }

        protected override void EnsureValidState()
        {
            var valid = Id != null && OwnerId != null && State switch
                                                         {
                                                             ClassifiedAdState.PendingReview => Title != null && Text != null && Price?.Amount > 0
                                                             , ClassifiedAdState.Active => Title != null && Text != null && Price?.Amount > 0
                                                                                           && ApprovedBy != null
                                                             , _ => true
                                                         };
            if (!valid) throw new InvalidEntityStateException(this, $"Post-checks failed in state {State}");
        }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case ClassifiedAdCreated e:
                    Id = new ClassifiedAdId(e.Id);
                    OwnerId = new UserId(e.OwnerId);
                    State = ClassifiedAdState.Inactive;
                    break;
                case ClassifiedAdTitleChanged e:
                    Title = ClassifiedAdTitle.FromString(e.Title);
                    break;
                case ClassifiedAdTextUpdated e:
                    Text = ClassifiedAdText.FromString(e.Text);
                    break;
                case ClassifiedAdPriceUpdated e:
                    Price = new Price(e.Price, e.Currency);
                    break;
                case ClassifiedAdSentForReview e:
                    State = ClassifiedAdState.PendingReview;
                    break;
            }
        }

        public void SetTitle(ClassifiedAdTitle title) => Apply(new ClassifiedAdTitleChanged {Id = Id, Title = title});

        public void UpdateText(ClassifiedAdText text) => Apply(new ClassifiedAdTextUpdated {Id = Id, Text = text});

        public void UpdatePrice(Price price) => Apply(new ClassifiedAdPriceUpdated {Id = Id, Price = price, Currency = price.Currency});

        public void RequestToPublish() => Apply(new ClassifiedAdSentForReview {Id = Id});
    }
}