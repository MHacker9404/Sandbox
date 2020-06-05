using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Marketplace.WebApi.Controllers.ClassifiedAds.QueryModels;
using Marketplace.WebApi.Controllers.ClassifiedAds.ReadModels;
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
        private readonly IAsyncDocumentSession _session;
        private readonly ILogger _logger;

        public QueryController(IAsyncDocumentSession session, ILogger logger)
        {
            _session = session;
            _logger = logger;
        }

        //[HttpGet]
        //[Route("list")]
        //public async Task<IActionResult> Get([FromQuery] GetPublishedClassifiedAds model) => await RequestHandler.HandleQuery(() => _session.Query(model), _logger);

        //[HttpGet]
        //[Route("my-ads")]
        //public async Task<IActionResult> Get([FromQuery] GetOwnersClassifiedAds model) => await RequestHandler.HandleQuery(() => _session.Query(model), _logger);

        //[HttpGet]
        //[ProducesResponseType( (int)HttpStatusCode.OK )]
        //[ProducesResponseType( (int)HttpStatusCode.NotFound )]
        //public async Task<IActionResult> Get([FromQuery] GetPublishedClassifiedAd model) => await RequestHandler.HandleQuery(() => _session.Query(model), _logger);
        [HttpGet]
        public Task<IActionResult> Get([FromQuery] GetPublishedClassifiedAd request) => RequestHandler.HandleQueryAsync(() => _session.Query(request), _logger);
    }
}