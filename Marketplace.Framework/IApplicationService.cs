using System.Threading.Tasks;

namespace Marketplace.Framework
{
    public interface IApplicationService
    {
        Task HandleAsync(object command);
    }
}
