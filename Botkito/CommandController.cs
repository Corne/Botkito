using Botkito.Commands;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Botkito
{
    public class CommandController
    {
        private readonly IEnumerable<ICommand> commands;
        private readonly Dictionary<string, List<ICommand>> map = new Dictionary<string, List<ICommand>>();

        public CommandController(IEnumerable<ICommand> commands)
        {
            this.commands = commands;
            foreach (var command in this.commands)
            {
                foreach (string key in command.Keys.Select(k => k.ToLower()))
                {
                    if (!map.TryGetValue(key, out List<ICommand> keyCommands))
                    {
                        map[key] = keyCommands = new List<ICommand>();
                    }

                    keyCommands.Add(command);
                }
            }
        }

        public async Task Hanlde(SocketMessage message)
        {
            if (string.IsNullOrWhiteSpace(message.Content))
                return;
            string arg = message.Content.Split(' ')[0].ToLower();

            if (map.TryGetValue(arg, out List<ICommand> keyCommands))
            {
                await Task.WhenAll(keyCommands.Select(c => c.Execute(message)));
            }
        }
    }
}
