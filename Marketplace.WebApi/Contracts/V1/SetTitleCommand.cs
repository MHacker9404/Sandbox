using System;

namespace Marketplace.WebApi.Contracts.V1
{
    public class SetTitleCommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
    }
}