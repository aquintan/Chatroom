using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Chatroom.App.Services
{
    using Contracts;

    public class BotService : IBotService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BotService> _logger;

        public BotService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetData(string user, string command)
        {
            var responseString = await _httpClient.GetStringAsync($"/api/Stock?user={user}&stockName={command}");

            return responseString;
        }
    }
}