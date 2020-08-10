using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Chatroom.Core.Interfaces;
using Chatroom.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Chatroom.Bot.Controllers
{
    using Contracts;

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ILogger<StockController> _logger;

        public StockController(ILogger<StockController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get(string user, string stockName, [FromServices] IServiceScopeFactory serviceScopeFactory)
        {
            _logger.LogDebug("Getting Stock info", stockName);

            _ = Task.Run(async () =>
            {
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<IStockService>();
                    var mqService = scope.ServiceProvider.GetRequiredService<IStockMessageSender>();
                    var result = await service.GetStockData(stockName);
                    mqService.SendStockMessage(new StockMessage()
                    {
                        User = user,
                        Message = result
                    });
                }
            });

            return Accepted();
        }
    }
}