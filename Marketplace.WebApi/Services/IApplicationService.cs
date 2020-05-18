using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.WebApi.Services
{
    public interface IApplicationService
    {
        Task HandleAsync(object command);
    }
}
