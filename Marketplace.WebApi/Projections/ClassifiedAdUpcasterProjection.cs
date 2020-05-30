using System;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Marketplace.Domain.ClassifiedAd.Events;
using Marketplace.Framework;
using Marketplace.WebApi.Infrastructure;

namespace Marketplace.WebApi.Projections
{
    public class ClassifiedAdUpcasterProjection : IProjection

    {
        private readonly IEventStoreConnection _connection;
        private readonly Func<Guid, string> _getUserPhoto;
        private const string _streamName = "UpcastedClassifiedAdEvents";

        public ClassifiedAdUpcasterProjection(IEventStoreConnection connection, Func<Guid, string> getUserPhoto)
        {
            _connection = connection;
            _getUserPhoto = getUserPhoto;
        }

        public async Task Project(object @event)
        {
            switch (@event)
            {
                case ClassifiedAdPublished e:
                    var photoUrl = _getUserPhoto(e.OwnerId);
                    var newEvent = new Upcasts.ClassifiedAdPublished
                                   {
                                       Id = e.Id, OwnerId = e.OwnerId, ApprovedBy = e.ApprovedBy, SellersPhotoUrl = photoUrl
                                   };
                    await _connection.AppendEvents(_streamName, ExpectedVersion.Any, newEvent);
                    break;
            }
        }
    }
}