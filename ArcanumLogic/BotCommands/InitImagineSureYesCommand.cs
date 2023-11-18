using Integrators.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.Abstractions;
using TgBot.DataSphere;

namespace ArcanumLogic.BotCommands
{
    public class InitImagineSureYesCommand : RootSendCommand
    {
        public override string Command { get; set; } = $"/{DispatcherCommandsEnum.init_imagine_sure_yes}";

        public override Task<string> ExecuteAsync(string? param, Update executionContext, IBotAdapter service)
        {
            var message = DispatcherCommandsEnum.init_imagine.ToString();
            return Task.FromResult(message);
        }

        public override async Task<string> ExecuteAfterAsync(string? param, Update executionContext, IBotAdapter service, IDispatcher dispatcher)
        {
            var result = await dispatcher.DispatchSimple<string, string>(DispatcherCommandsEnum.init_command.ToString(), param ?? string.Empty);
            result = await base.ExecuteAfterAsync(result, executionContext, service, dispatcher);
            return result ?? string.Empty;
        }
    }
}
