using Discord;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amonikano
{
    /// <summary>
    /// All of these constants are supposed to be kept secret, but whatever, we don't need secret do we?
    /// </summary>
    public static class bot_const //TO HELL WITH NAMING PROBLEMS, I DON'T CARE
    {
        public const string client_id = "477149586851364867"; //I only all caps macros
        public const string client_secret = "QC6QmXIgHG1LwW9XrqVs5cvXln4PeRPQ";
        public const string token = "NDc3MTQ5NTg2ODUxMzY0ODY3.Dk4JiA.6I0tIiF7WUvG_24OEEhAck996cI____DELETETHIS_____";
    }




    class Program
    {
        static void Main(string[] args)
            => //This symbol is a lambda expression, search it up if you have to
            new Program().MainAsync(args).GetAwaiter().GetResult();

        /**
         * Basically, this calls an asynchronous main loop.
         * Because we assume the program run so fast that it runs asynchronously, there's nothing wrong with this.
        */
        /**
         * From here, I'll try to make the code looks neat and explanatory :)
         * */
        public async Task MainAsync(string[] args)
        {
            //This is from another namespace. I don't intend to include a using namespace because it would look messy on the long run
            Discord.WebSocket.
                DiscordSocketClient client;
            client = new Discord.WebSocket.
                DiscordSocketClient();

            client.Log += on_client_log; //client.Log is like a delegate function (a list of function, or an event)
            client.MessageReceived += on_message_received;
            await client.LoginAsync(TokenType.Bot, bot_const.token);

            await client.StartAsync();

            await Task.Delay(-1);
        }

        /// <summary>
        /// Gets called whenever the client logs something
        /// This basically logs out on the bot console (not in discord, but prolly on my computer)
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private Task on_client_log(LogMessage msg) //not async because idk
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets called whenever the client receives a message from discord
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private async Task on_message_received(Discord.WebSocket.SocketMessage msg)
        {
            if (msg.Content == "!ping")
            {
                string output = "";
                DateTimeOffset created = msg.CreatedAt;
                DateTimeOffset msg_out = msg.Timestamp;
                TimeSpan delta = created - msg_out;
                output = "The message, was created at " + created.ToString()
                    + " and was out at " + msg_out.ToString()
                    + " which took " + delta.Milliseconds + "ms.";
                await msg.Channel.SendMessageAsync("pong;\n" + output);
            }
        }
    }
}
