using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Chatroom.App.Models;
using Chatroom.App.Services;
using NUnit.Framework;

namespace Chatroom.Tests
{
    public class MessageProcessorServiceUnitTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("/stock=AAPPL.US", MessageType.Command)]
        [TestCase("/stock=Stock", MessageType.Command)]
        [TestCase("/stock=Stock.COP", MessageType.Command)]
        [TestCase("/stock=AAPPL.US", MessageType.Command)]
        [TestCase("/stockAAPPL.US", MessageType.Message)]
        [TestCase("/Hola", MessageType.Message)]
        [TestCase("Hola", MessageType.Message)]
        [TestCase("Hola todos", MessageType.Message)]
        [TestCase("", MessageType.Message)]
        [TestCase(" ", MessageType.Message)]
        public async Task MessageProcessorGetStockFromCommandTest(string input, MessageType expectedResult)
        {
            var processor = new MessageProcessorService();

            var result = processor.GetMessageType(input);

            Assert.IsTrue(result == expectedResult);
        }

        [TestCase("/stock=AAPPL.US", "AAPPL.US")]
        [TestCase("/stock=StockValue", "StockValue")]
        [TestCase("/stockAAPPL.US", "")]
        [TestCase("/Hola", "")]
        public async Task MessageProcessorGetStockFromCommandTest(string input, string expectedResult)
        {
            var processor = new MessageProcessorService();

            var result = processor.GetStockFromCommand(input);

            Assert.IsTrue(result == expectedResult);
        }
    }
}
