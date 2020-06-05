using System.Threading.Tasks;
using EventStore.ClientAPI;

namespace Marketplace.Framework
{
    public interface ICheckpointStore
    {
        Task<Position> GetCheckpointAsync();
        Task StoreCheckpointAsync(Position checkPoint);
    }
}