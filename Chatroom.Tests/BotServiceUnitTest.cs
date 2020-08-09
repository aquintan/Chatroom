using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Chatroom.App.Services;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace Chatroom.Tests
{
    public class BotServiceUnitTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("AAPL.US quote is 444.45 $ per share", "AAPL.US quote is 444.45 $ per share")]
        public async Task StockServiceTest(string input, string expectedResult)
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
                    Content = new StringContent(input),
                })
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };

            var service = new BotService(httpClient);
            var response = await service.GetData(input);

            Assert.IsTrue(response == expectedResult);
        }
    }
}
