using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Framework
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}
