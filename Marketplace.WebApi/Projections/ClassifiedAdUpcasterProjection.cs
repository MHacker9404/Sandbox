using System;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Marketplace.Domain.ClassifiedAd.Events;
using Marketplace.Framework;
using Marketplace.WebApi.Infrastructure;
using ILogger = Serilog.ILogger;

namespace Marketplace.WebApi.Projections
{
    public class ClassifiedAdUpcasterProjection : IProjection

    {
        private readonly IEventStoreConnection _connection;
        private readonly Func<Guid, Task<string>> _getUserPhoto;
        private readonly ILogger _logger;
        private const string _streamName = "UpcastedClassifiedAdEvents";

        public ClassifiedAdUpcasterProjection(IEventStoreConnection connection, Func<Guid, Task<string>> getUserPhoto, ILogger logger)
        {
            _connection = connection;
            _getUserPhoto = getUserPhoto;
            _logger = logger;
        }

        public async Task Project(object @event)
        {
            _logger.Debug($"Projecting event {@event.GetType().Name}");
            switch (@event)
            {
                case ClassifiedAdPublished e:
                    var photoUrl = await _getUserPhoto(e.OwnerId);
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