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
    public class InitAllCommand : RootSendCommand
    {
        public override string Command { get => $"/{DispatcherCommandsEnum.init_all}_sure_yes"; set => base.Command = value; }
        public override async Task<string> ExecuteAfterAsync(string? param, Update executionContext, IBotAdapter service, IDispatcher dispatcher)
        {
            param = DispatcherCommandsEnum.init_users.ToString();
            _ = await dispatcher.DispatchSimple<string, string>(DispatcherCommandsEnum.init_command.ToString(), param ?? string.Empty);
            param = DispatcherCommandsEnum.init_imagine.ToString();
            _ = await dispatcher.DispatchSimple<string, string>(DispatcherCommandsEnum.init_command.ToString(), param ?? string.Empty);
            param = DispatcherCommandsEnum.init_tree.ToString();
            _ = await dispatcher.DispatchSimple<string, string>(DispatcherCommandsEnum.init_command.ToString(), param ?? string.Empty);
            param = DispatcherCommandsEnum.init_fabricas.ToString();
            _ = await dispatcher.DispatchSimple<string, string>(DispatcherCommandsEnum.init_command.ToString(), param ?? string.Empty);
            param = "Все вроде создано, но это не точно";

            return await base.ExecuteAfterAsync(param, executionContext, service, dispatcher);
        }
    }
}
