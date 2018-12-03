using Discord.WebSocket;
using System.Threading.Tasks;

namespace Botkito.Commands
{
    public class PingPong : ICommand
    {
        private static readonly string[] keys = new[] { "!ping" };

        public string[] Keys => keys;

        public Task Execute(SocketMessage message)
        {
            return message.Channel.SendMessageAsync("pong!");
        }
    }
}
