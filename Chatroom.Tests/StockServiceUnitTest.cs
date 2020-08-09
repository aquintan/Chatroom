using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Chatroom.Bot.Services;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace Chatroom.Tests
{
    public class StockServiceUnitTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("AAPL.US", "Symbol,Date,Time,Open,High,Low,Close,Volume\r\nAAPL.US,2020-08-07,22:00:02,452.82,454.7,441.17,444.45,49511403", "AAPL.US quote is 444.45 $ per share")]
        [TestCase("AAPL.US", "Symbol,Date,Time,Open,High,Low,Close,Volume\r\nAAPL.US,2020-08-07,22:00:02,452.82,454.7,441.17,N/D,49511403", "AAPL.US is not a valid Stock Code.")]
        [TestCase("AAPL.US", "Symbol,Date,Time,Open,High,Low,Close,Volume", "No data provided for stock code AAPL.US")]
        public async Task StockServiceTest(string input, string stooqResult, string expectedResult)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(stooqResult),
                })
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };

            var service = new StockService(httpClient);
            var response = await service.GetStockData(input);

            Assert.IsTrue(response == expectedResult);
        }
    }
}