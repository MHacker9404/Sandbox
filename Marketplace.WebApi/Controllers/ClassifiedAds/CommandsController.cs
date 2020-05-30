using System;
using System.Threading.Tasks;
using Marketplace.WebApi.Contracts.V1.ClassifiedAd;
using Marketplace.WebApi.Infrastructure;
using Marketplace.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Marketplace.WebApi.Controllers.ClassifiedAds
{
    [Route("api/ads")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ClassifiedAdAppService _appService;
        private readonly ILogger _logger;

        public CommandsController(ClassifiedAdAppService appService, ILogger logger)
        {
            _appService = appService;
            _logger = logger;
        }


        [HttpPost]
        public async Task<IActionResult> Post(CreateAdCommand command) => await RequestHandler.HandleCommand(command, _appService.HandleAsync, _logger);

        [Route("title")]
        [HttpPut]
        public async Task<IActionResult> Put(SetTitleCommand command) => await RequestHandler.HandleCommand(command, _appService.HandleAsync, _logger);

        [Route("text")]
        [HttpPut]
        public async Task<IActionResult> Put(UpdateTextCommand command) => await RequestHandler.HandleCommand( command, _appService.HandleAsync, _logger );

        [Route("price")]
        [HttpPut]
        public async Task<IActionResult> Put(UpdatePriceCommand command) => await RequestHandler.HandleCommand( command, _appService.HandleAsync, _logger );

        [Route("request-to-publish")]
        [HttpPut]
        public async Task<IActionResult> Put(RequestToPublishCommand command) => await RequestHandler.HandleCommand( command, _appService.HandleAsync, _logger );

        [Route("publish")]
        [HttpPut]
        public async Task<IActionResult> Put(PublishCommand command) => await RequestHandler.HandleCommand( command, _appService.HandleAsync, _logger );
    }
}