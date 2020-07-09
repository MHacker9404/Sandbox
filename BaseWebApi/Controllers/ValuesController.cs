using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BaseWebApi.Controllers
{
    [Route( "api/[controller]" )]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger _logger;

        public ValuesController(ILogger logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("values")]
        public async Task<IActionResult> Values() => new OkResult();
    }
}
