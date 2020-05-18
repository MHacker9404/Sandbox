using System;

namespace Marketplace.WebApi.Contracts.V1
{
    public class CreateAdCommand
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
    }
}
