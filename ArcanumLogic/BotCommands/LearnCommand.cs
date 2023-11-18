using ArcanumLogic.Dtos;
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

public class LearnCommand : VipCommand
{
    public override string Command { get => $"/{DispatcherCommandsEnum.learn}"; set => base.Command = value; }
    public override Task<string> ExecuteAsync(string? param, Update executionContext, IBotAdapter service)
    {
        return Task.FromResult(param ?? string.Empty);
    }

    public override async Task<string> ExecuteAfterAsync(string? param, Update executionContext, IBotAdapter service, IDispatcher dispatcher)
    {
        if (string.IsNullOrEmpty(param))
            return $"Неверные параметры команды. Вводите /{DispatcherCommandsEnum.learn} айди_схемы";
        var dto = new LearnDto { TgId = BotHelper.GetChatId(executionContext), SearchKey = param };
        var response = await dispatcher.DispatchSimple<LearnDto, string>(DispatcherCommandsEnum.learn.ToString(), dto);
        return await base.ExecuteAfterAsync(response, executionContext, service, dispatcher);
    }
}
