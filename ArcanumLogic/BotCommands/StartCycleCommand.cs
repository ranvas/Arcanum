using Integrators.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.Abstractions;
using TgBot.DataSphere;

namespace ArcanumLogic.BotCommands;

public class StartCycleCommand : RootSendCommand
{
    public override string Command { get; set; } = $"/{DispatcherCommandsEnum.start_cycle}";
    public override async Task<string> ExecuteAfterAsync(string? param, Update executionContext, IBotAdapter service, IDispatcher dispatcher)
    {
        var result = await dispatcher.DispatchSimple<string, string>(DispatcherCommandsEnum.start_cycle.ToString(), param ?? string.Empty);
        return await base.ExecuteAfterAsync(result, executionContext, service, dispatcher);
    }
}
