namespace Chatroom.Core.Interfaces
{
    using Models;

    public interface IStockMessageSender
    {
        void SendStockMessage(StockMessage stockMessage);
    }
}