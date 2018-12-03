using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Botkito.Commands
{
    public interface ICommand
    {
        string[] Keys { get; }
        Task Execute(SocketMessage message);
    }
}
