using System;
using System.Threading.Tasks;
using Marketplace.WebApi.Contracts.V1.UserProfile;
using Marketplace.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Marketplace.WebApi.Controllers.UserProfile
{
    [Route( "api/user" )]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly UserProfileAppService _appService;
        private readonly ILogger _logger;

        public CommandsController(UserProfileAppService appService, ILogger logger)
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
        public async Task<IActionResult> Post(RegisterUser command) => await HandleRequest(command, _appService.HandleAsync);

        [Route("full-name")]
        [HttpPut]
        public async Task<IActionResult> Put(UpdateUserFullName command) => await HandleRequest(command, _appService.HandleAsync);

        [Route("display-name")]
        [HttpPut]
        public async Task<IActionResult> Put(UpdateUserDisplayName command) => await HandleRequest(command, _appService.HandleAsync);

        [Route("photo")]
        [HttpPut]
        public async Task<IActionResult> Put(UpdateUserProfilePhoto command) => await HandleRequest(command, _appService.HandleAsync);
    }
}
