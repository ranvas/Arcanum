using ArcanumLogic.Dtos;
using Integrators.Abstractions;
using Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using TgBot.Abstractions;
using TgBot.DataSphere;

namespace ArcanumLogic.BotCommands;

public class GetStatusCommand : LoginCommand
{
    public override string Command { get; set; } = $"/{DispatcherCommandsEnum.status}";

    public override async Task<string> ExecuteAfterAsync(string? param, Update executionContext, IBotAdapter service, IDispatcher dispatcher)
    {
        var tgId = BotHelper.GetChatId(executionContext);
        var request = new RequestBase() { TgId = tgId };
        var response = await dispatcher.DispatchSimple<RequestBase, ResponseBase>(DispatcherCommandsEnum.status.ToString(), request);

        var message = await base.ExecuteAfterAsync(response?.ResultMessage ?? "unknown error", executionContext, service, dispatcher);
        return message;
    }
}
