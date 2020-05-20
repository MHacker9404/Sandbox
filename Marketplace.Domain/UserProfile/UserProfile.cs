using System;
using Marketplace.Domain.Shared;
using Marketplace.Domain.UserProfile.Events;
using Marketplace.Framework;

namespace Marketplace.Domain.UserProfile
{
    public class UserProfile : AggregateRoot<UserId>
    {
        public UserProfile(UserId id, FullName fullName, DisplayName displayName) =>
            Apply(new UserRegistered
                  {
                      UserId = id, FullName = fullName, DisplayName = displayName
                  });

        //  for persistence in RavenDB
        private string DbId
        {
            get => $"UserProfile/{Id.Value}";
            set { }
        }

        //  state properties
        public FullName FullName { get; private set; }
        public DisplayName DisplayName { get; private set; }
        public string PhotoUrl { get; private set; }

        public void UpdateFullName(FullName fullName) =>
            Apply(new UserFullNameUpdated
                  {
                      UserId = Id, FullName = fullName
                  });

        public void UpdateDisplayName(DisplayName displayName) =>
            Apply(new UserDisplayNameUpdated
                  {
                      UserId = Id, DisplayName = displayName
                  });

        public void UpdateProfilePhoto(string photoUri) =>
            Apply(new ProfilePhotoUploaded
                  {
                      UserId = Id, PhotoUrl = photoUri
                  });

        protected override void EnsureValidState() { }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case UserRegistered e:
                    Id = UserId.FromGuid(e.UserId);
                    FullName = FullName.FromString(e.FullName);
                    //DisplayName = DisplayName.FromString(e.DisplayName);
                    break;

                case UserDisplayNameUpdated e:
                    //DisplayName = DisplayName.FromString(e.DisplayName);
                    break;

                case UserFullNameUpdated e:
                    FullName = FullName.FromString(e.FullName);
                    break;

                case ProfilePhotoUploaded e:
                    PhotoUrl = e.PhotoUrl;
                    break;
            }
        }
    }
}