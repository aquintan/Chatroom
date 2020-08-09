using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatroom.Bot.Contracts
{
    public interface IStockService
    {
        Task<string> GetStockData(string name);
    }
}
