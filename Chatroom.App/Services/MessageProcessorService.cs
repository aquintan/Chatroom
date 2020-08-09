using Chatroom.App.Contracts;
using Chatroom.App.Models;
using System.Text.RegularExpressions;

namespace Chatroom.App.Services
{
    /// <summary>
    /// CommandParserService class.
    /// </summary>
    public class MessageProcessorService : IMessageProcessorService
    {
        // This regex string recognizes a command and groups the parts
        private const string regEx = @"^(\/stock=)([a-zA-Z0-9:\.^_]+)$";

        public string GetStockFromCommand(string text)
        {
            var result = "";

            Match match = Regex.Match(text, regEx, RegexOptions.Singleline);

            if (match.Success)
            {
                result = match.Groups[2].Value;
            }

            return result;
        }

        public MessageType GetMessageType(string text)
        {
            return (Regex.IsMatch(text, regEx, RegexOptions.Singleline)) ? MessageType.Command : MessageType.Message;
        }
    }
}