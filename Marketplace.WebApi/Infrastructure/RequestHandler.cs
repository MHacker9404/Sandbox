using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Marketplace.WebApi.Infrastructure
{
    internal static class RequestHandler
    {
        internal static async Task<IActionResult> HandleCommand<T>(T command, Func<T, Task> handler, ILogger logger)
        {
            try
            {
                logger.Debug("{@Command}", command);
                await handler(command);
                return new OkResult();
            }
            catch (Exception e)
            {
                logger.Error(e, "Error handling the command");
                return new BadRequestObjectResult(new {error = e.Message, stackTrace = e.StackTrace});
            }
        }

        internal static async Task<IActionResult> HandleQueryAsync<TModel>(Func<Task<TModel>> query, ILogger logger)
        {
            try
            {
                logger.Debug("{@Command}", query);
                return new OkObjectResult(await query());
            }
            catch (Exception e)
            {
                logger.Error(e, "Error handling the query");
                return new BadRequestObjectResult(new {error = e.Message, stackTrace = e.StackTrace});
            }
        }

        internal static IActionResult HandleQuery<TModel>(Func<TModel> query, ILogger logger)
        {
            try
            {
                logger.Debug("{@Command}", query);
                var result = query();
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                logger.Error(e, "Error handling the query");
                return new BadRequestObjectResult(new {error = e.Message, stackTrace = e.StackTrace});
            }
        }
    }
}
