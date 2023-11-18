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
    public class AdminAllSchemesCommand : RootSendCommand
    {
        public override string Command { get => $"/{DispatcherCommandsEnum.admin_all_schemes}"; set => base.Command = value; }

        public override async Task<string> ExecuteAfterAsync(string? param, Update executionContext, IBotAdapter service, IDispatcher dispatcher)
        {
            var message = await dispatcher.DispatchSimple<string>(DispatcherCommandsEnum.admin_all_schemes.ToString());
            return await base.ExecuteAfterAsync(message, executionContext, service, dispatcher);
        }
    }
}
