using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace Amonikano.Library.AmonikanoCommands
{
    
    public class CoreModule : ModuleBase
    {
        [Command("say"),Summary("Say a message")]
        public async Task Say
            (
            [Remainder, Summary("remainder -> speech")] string speech
            )
        {
            await ReplyAsync(speech);
        }

        [Command("help"), Summary("Throws help")]
        public async Task Help
            (
            [Remainder, Summary("remainder -> args")] string args
            )
        {
            if (!_help_initialized)
            {
                await InitializeHelp();
                _help_initialized = true;
            }
            else
            {
                await ReplyAsync(_help);
            }
        }
        private static string mod_info_in_string(string name, string[] aliases, string summary)
        {
            return "Group " + name + " (prefix: " + aliases[0] + ", aliases:" + string.Join(", ", aliases, 1, aliases.Length - 1) + "): " + summary + "\n{";
        }
        private static string command_info_in_string(string name, string[] alias, string summary)
        {
            return " _"+ name + 
                (alias != null ? 
                " ("+ string.Join(", ",alias)+")"   :  "(no alias)")+ ": "
                + summary +"\n";
        }
        private static string _help = "";
        private static bool _help_initialized = false;
        public const string help_already_initialized = "help string has been initialized, and has not been changed since then";
        public async Task InitializeHelp()
        {
#if DEBUG
            DateTime now = DateTime.Now;
#endif
            CommandService service = Program.command_serv;
#if DEBUG
            DateTime then = DateTime.Now;
            await ReplyAsync("Getting service from Program.command_serv took " + (then - now).TotalMilliseconds + " ms.");
            now = DateTime.Now;
#endif
            var mods = service.Modules;
#if DEBUG
            then = DateTime.Now;
            await ReplyAsync("operation: var mods = service.Modules took " + (then - now).TotalMilliseconds + " ms.");
            bool got_to_foreach_loop = false;
            now = DateTime.Now;
#endif
            foreach (var mod in mods)
            {
#if DEBUG
                if (!got_to_foreach_loop)
                {
                    got_to_foreach_loop = true;
                    then = DateTime.Now;
                    await ReplyAsync("Time took to get to foreach(var mod in mods): " + (then - now).TotalMilliseconds + " ms.");
                }
                now = DateTime.Now;
#endif
                string[] aliases_array = mod.Aliases.ToArray();
#if DEBUG
                then = DateTime.Now;
                await ReplyAsync("operation: mod.Aliases.ToArray() took " + (then - now).TotalMilliseconds + " ms.");
                bool got_to_commands_foreach = false;
                now = DateTime.Now;
#endif
                string h = mod_info_in_string(mod.Name, aliases_array, mod.Summary);
                _help += h+"\n";
                await ReplyAsync(h);
                CommandInfo[] commands = mod.Commands.ToArray();
                foreach (CommandInfo command in commands)
                {
#if DEBUG
                    if (!got_to_commands_foreach)
                    {
                        got_to_commands_foreach = true;
                        then = DateTime.Now;
                        await ReplyAsync("Time took to get to foreach(var command in mod.Commands): " + (then - now).TotalMilliseconds + " ms.");
                    }
                    now = DateTime.Now;
#endif
                    string[] alias = command.Aliases.ToArray();
#if DEBUG
                    then = DateTime.Now;
                    await ReplyAsync("operation: command.Aliases.ToArray() took " + (then - now).TotalMilliseconds + " ms.");
#endif
                    h = command_info_in_string(command.Name, alias, command.Summary);
                    _help += h;
                    await ReplyAsync(h);
                }
                _help += "}";
                await ReplyAsync("}");
            }
        }

    }
}
