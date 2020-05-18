using System.Threading.Tasks;

namespace Marketplace.WebApi.Services {
    public interface IHandleCommand<in T>
    {
        Task HandleAsync(T command);
    }
}