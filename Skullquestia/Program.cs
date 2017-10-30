using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Skullquestia
{
    class Program
    {
        static void Main(string[] args)
        {
            // Wrap in a try-catch block because otherwise it only recognizes this line as the error
            try
            {
                // Starts the bot. Just trust me on this.
                //   Discord.Net is entirely asynchronous, so this runs Start() asynchronously
                new Program().Start().GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
        }

        // Bot's Discord Client
        private DiscordSocketClient m_client;

        // Stuff for Commands (just believe me here)
        private IServiceProvider m_services;
        private readonly IServiceCollection m_map = new ServiceCollection();
        private readonly CommandService m_commands = new CommandService();



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            // Output beginning of loading phase

            // Output beginning of loading phase
            await BotFunc.Log(new LogMessage(LogSeverity.Info, "Program", "Loading..."));


            // Instantiate client object
            m_client = new DiscordSocketClient();


            // Add Log method to client's inherent Log method
            m_client.Log += BotFunc.Log;


            // Output beginning of command loading phase
            await BotFunc.Log(new LogMessage(LogSeverity.Info, "Program", "Loading commands..."));


            // Initialize Commands
            await InitCommands();


            //Output end of command loading phase
            await BotFunc.Log(new LogMessage(LogSeverity.Info, "Program", "Commands loaded."));

            // Set start of uptime
            BotFunc.start_time = DateTime.Now;

            // Output end of loading phase
            await BotFunc.Log(new LogMessage(LogSeverity.Info, "Program", "Loading finished. Starting bot..."));


            // Login and start the bot
            await m_client.LoginAsync(TokenType.Bot, BotInfo.bot_token);
            await m_client.StartAsync();

            // Give time to connect
            await Task.Delay(5000);


            // Set humorous game
            await m_client.SetGameAsync("Punish the Users: Remastered");


            // Keep task running until program is closed
            await Task.Delay(-1);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task InitCommands()
        {
            await m_commands.AddModulesAsync(Assembly.GetEntryAssembly());

            m_services = m_map.BuildServiceProvider();

            m_client.MessageReceived += HandleCommandAsync;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var msg = arg as SocketUserMessage;
            if (msg == null) return;

            int pos = 0;

            if (msg.HasCharPrefix(BotInfo.command_prefix, ref pos))
            {
                await BotFunc.Log(new LogMessage(LogSeverity.Info, "CommandHandler", "Received command: " + arg.Content));

                var context = new SocketCommandContext(m_client, msg);

                var result = await m_commands.ExecuteAsync(context, pos, m_services);

                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                    await msg.Channel.SendMessageAsync(result.ErrorReason);
            }
        }
    }
}
