namespace Chatroom.App.Contracts
{
    using Chatroom.App.Models;

    public interface IMessageProcessorService
    {
        /// <summary>
        /// Parses a command string to extract the stock value
        /// </summary>
        /// <param name="text">The text to parse</param>
        /// <returns>The stock name</returns>
        string GetStockFromCommand(string text);

        /// <summary>
        /// Determines the type of Message
        /// </summary>
        /// <param name="text">The text to check</param>
        /// <returns>Type of the message</returns>
        MessageType GetMessageType(string text);
    }
}