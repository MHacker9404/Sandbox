using System.Threading.Tasks;
using LamarCodeGeneration;
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

        [HttpPost]
        public async Task<IActionResult> Post(CreateAdCommand command)
        {
            _logger.Debug("{@Command}", command);

            _logger.Verbose( "Post method - verbose" );
            _logger.Debug( "Post method - debug" );
            _logger.Information( "Post method - info" );
            _logger.Warning( "Post method - warn" );
            _logger.Error( "Post method - error" );
            _logger.Fatal( "Post method - fatal" );

            await _appService.HandleAsync(command);

            return Ok();
        }

        [Route("name")]
        [HttpPut]
        public async Task<IActionResult> Put(SetTitleCommand command)
        {
            _logger.Debug("{@Command}", command);


            await _appService.HandleAsync(command);

            return Ok();
        }

        [Route("text")]
        [HttpPut]
        public async Task<IActionResult> Put(UpdateTextCommand command)
        {
            _logger.Debug("{@Command}", command);


            await _appService.HandleAsync(command);

            return Ok();
        }

        [Route("price")]
        [HttpPut]
        public async Task<IActionResult> Put(UpdatePriceCommand command)
        {
            _logger.Debug("{@Command}", command);


            await _appService.HandleAsync(command);

            return Ok();
        }

        [Route("publish")]
        [HttpPut]
        public async Task<IActionResult> Put(RequestToPublishCommand command)
        {
            _logger.Debug("{@Command}", command);


            await _appService.HandleAsync(command);

            return Ok();
        }
    }
}