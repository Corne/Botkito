using Autofac;
using Botkito.Configuration;
using Discord;
using Discord.WebSocket;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Botkito
{
    class Program
    {
        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            var client = new DiscordSocketClient();
            var container = ComponentRegister.BuildContainer();

            var commandcontroller = container.Resolve<CommandController>();

            client.Log += Log;
            client.MessageReceived += commandcontroller.Hanlde;

            string token = File.ReadAllText("token.txt");
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
