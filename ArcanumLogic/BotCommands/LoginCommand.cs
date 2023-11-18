using ArcanumLogic.Dtos;
using Google.Apis.Sheets.v4.Data;
using Integrators.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Telegram.Bot.Types;
using TgBot.Abstractions;
using TgBot.DataSphere;

namespace ArcanumLogic.BotCommands;

public class LoginCommand : DefaultCommand
{
    public override string Command { get; set; } = $"/{DispatcherCommandsEnum.start}";
    public override async Task<bool> ExecuteBeforeAsync(string? param, Update executionContext, IBotAdapter service, IDispatcher dispatcher)
    {
        if (!await base.ExecuteBeforeAsync(param, executionContext, service, dispatcher))
            return false;
        var tgId = BotHelper.GetChatId(executionContext);
        var tgName = BotHelper.GetUserName(executionContext);
        var request = new RequestBase() { TgId = tgId, TgName = tgName };
        return await dispatcher.DispatchSimple<RequestBase, bool>(DispatcherCommandsEnum.start.ToString(), request);
    }

    public override Task<string> ExecuteAsync(string? param, Update executionContext, IBotAdapter service)
    {
        var tgId = BotHelper.GetChatId(executionContext);
        var tgName = BotHelper.GetUserName(executionContext);
        var request = new RequestBase() { TgId = tgId, TgName = tgName };
        var sb = new StringBuilder();
        sb.AppendLine("Вы авторизованы как игрок:");
        sb.AppendLine($"/{DispatcherCommandsEnum.status.ToString()} - общий статус персонажа");
        sb.AppendLine($"Если вы иссследователь, введите /login_vip");
        return base.ExecuteAsync(sb.ToString(), executionContext, service);
    }

    public override async Task<string> ExecuteAfterAsync(string? param, Update executionContext, IBotAdapter service, IDispatcher dispatcher)
    {

        return await base.ExecuteAfterAsync(param, executionContext, service, dispatcher);
    }
}
