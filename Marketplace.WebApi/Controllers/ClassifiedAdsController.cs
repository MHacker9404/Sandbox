using System;
using System.Threading.Tasks;
using Marketplace.WebApi.Contracts.V1;
using Marketplace.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Marketplace.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassifiedAdsController : ControllerBase
    {
        private readonly ClassifiedAdAppService _appService;
        private readonly ILogger _logger;

        public ClassifiedAdsController(ClassifiedAdAppService appService, ILogger logger)
        {
            _appService = appService;
            _logger = logger;
        }

        private async Task<IActionResult> HandleRequest<T>(T command, Func<T, Task> handler)
        {
            try
            {
                _logger.Debug("{@Command}", command);
                await handler(command);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error handling the request");
                return new BadRequestObjectResult(new {error = e.Message, stackTrace = e.StackTrace});
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateAdCommand command) => await HandleRequest(command, _appService.HandleAsync);

        [Route("name")]
        [HttpPut]
        public async Task<IActionResult> Put(SetTitleCommand command) => await HandleRequest(command, _appService.HandleAsync);

        [Route("text")]
        [HttpPut]
        public async Task<IActionResult> Put(UpdateTextCommand command) => await HandleRequest(command, _appService.HandleAsync);

        [Route("price")]
        [HttpPut]
        public async Task<IActionResult> Put(UpdatePriceCommand command) => await HandleRequest(command, _appService.HandleAsync);

        [Route("publish")]
        [HttpPut]
        public async Task<IActionResult> Put(RequestToPublishCommand command) => await HandleRequest(command, _appService.HandleAsync);
    }
}