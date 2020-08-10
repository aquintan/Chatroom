using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatroom.App.Contracts
{
    public interface IBotService
    {
        Task<string> GetData(string user, string command);
    }
}
