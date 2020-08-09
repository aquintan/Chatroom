using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;
using Chatroom.Bot.Models;
using Chatroom.Core.Utils;
using CsvHelper;

namespace Chatroom.Bot.Services
{
    using Contracts;

    public class StockService : IStockService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<StockService> _logger;

        public StockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetStockData(string name)
        {
            var url = $"?s={name}&f=sd2t2ohlcv&h&e=csv";
            var responseString = await _httpClient.GetStringAsync(url);
            var sq = ProcessData(responseString);
            var quoteMessage = String.Empty;

            if (sq.Count == 0)
            {
                quoteMessage = $"No data provided for stock code {name}";
            }
            else
            {
                var record = sq[0];

                if (record.Close.Equals("N/D"))
                {
                    quoteMessage = $"{name} is not a valid Stock Code.";
                }
                else
                {
                    quoteMessage = $"{name} quote is {record.Close} $ per share";
                }
            }

            return quoteMessage;
        }

        private List<StockQuote> ProcessData(string data)
        {
            TextReader reader = new StreamReader(StreamUtils.GenerateStreamFromString(data));
            var csvReader = new CsvReader(reader, CultureInfo.CurrentCulture);
            var records = csvReader.GetRecords<StockQuote>();

            return records.ToList();
        }
    }
}