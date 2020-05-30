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
        private readonly HashSet<ClassifiedAdDetails> _details;
        private readonly ILogger _logger;

        public QueryController(HashSet<ClassifiedAdDetails> details, ILogger logger)
        {
            _details = details;
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
        public IActionResult Get([FromQuery] GetPublishedClassifiedAd request) => RequestHandler.HandleQuery(() => _details.Query(request), _logger);
    }
}