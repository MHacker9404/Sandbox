using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MediatRDemo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediatRDemo.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContactsController(IMediator mediator) => _mediator = mediator;

        [HttpGet("{id}")]
        public async Task<Contact> GetContact([FromRoute] Query query) => await this._mediator.Send(query);

        #region Nested Classes

        public class Query : IRequest<Contact>
        {
            public int Id { get; set; }
        }

        public class ContactHandler : IRequestHandler<Query, Contact>
        {
            private readonly ContactsContext _db;

            public ContactHandler(ContactsContext db) => _db = db;

            /// <inheritdoc />
            public Task<Contact> Handle(Query request, CancellationToken cancellationToken) =>
                _db.Contacts
                   .Where(c => c.Id == request.Id)
                   .SingleOrDefaultAsync(cancellationToken);
        }

        #endregion
    }
}