using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Chatroom.Bot.Controllers
{
    using Contracts;

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ILogger<StockController> _logger;
        private readonly IStockService _service;

        public StockController(IStockService service, ILogger<StockController> logger)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<string> Get(string stockName)
        {
            _logger.LogDebug("Getting Stock info", stockName);
            var result = await _service.GetStockData(stockName);

            return result;
        }
    }
}