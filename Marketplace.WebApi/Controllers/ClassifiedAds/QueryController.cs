using System;
using System.Net;
using System.Threading.Tasks;
using Marketplace.WebApi.Controllers.ClassifiedAds.QueryModels;
using Marketplace.WebApi.Infrastructure;
using Marketplace.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using Serilog;

namespace Marketplace.WebApi.Controllers.ClassifiedAds
{
    [Route("api/ads")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IAsyncDocumentSession _session;

        public QueryController(IAsyncDocumentSession session, ILogger logger)
        {
            _session = session;
            _logger = logger;
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> Get([FromQuery] GetPublishedClassifiedAds model) => await RequestHandler.HandleQuery(() => _session.Query(model), _logger);

        [HttpGet]
        [Route("my-ads")]
        public async Task<IActionResult> Get([FromQuery] GetOwnersClassifiedAds model) => await RequestHandler.HandleQuery(() => _session.Query(model), _logger);

        [HttpGet]
        [ProducesResponseType( (int)HttpStatusCode.OK )]
        [ProducesResponseType( (int)HttpStatusCode.NotFound )]
        public async Task<IActionResult> Get([FromQuery] GetPublishedClassifiedAd model) => await RequestHandler.HandleQuery(() => _session.Query(model), _logger);
    }
}