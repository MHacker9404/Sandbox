using System;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Marketplace.Framework;
using Raven.Client.Documents.Session;
using ILogger = Serilog.ILogger;

namespace Marketplace.WebApi.Infrastructure
{
    public class RavenDbCheckpointStore : ICheckpointStore
    {
        private readonly string _checkPointName;
        private readonly Func<IAsyncDocumentSession> _getSession;
        private readonly ILogger _logger;

        public RavenDbCheckpointStore(Func<IAsyncDocumentSession> getSession, ILogger logger)
        {
            _getSession = getSession;
            _checkPointName = string.Empty;
            _logger = logger;
        }

        public async Task<Position> GetCheckpointAsync()
        {
            _logger.Debug($"");

            using var session = _getSession();
            var checkPoint = await session.LoadAsync<Checkpoint>(_checkPointName);
            return checkPoint?.Position ?? Position.Start;
        }

        public async Task StoreCheckpointAsync(Position position)
        {
            _logger.Debug($"");

            using var session = _getSession();
            var checkPoint = await session.LoadAsync<Checkpoint>(_checkPointName);
            if (checkPoint == null)
            {
                checkPoint = new Checkpoint();
                await session.StoreAsync(checkPoint);
            }

            checkPoint.Position = position;
            await session.SaveChangesAsync();
        }
    }
}