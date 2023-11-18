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

public class LearnNeutralCommand : VipCommand
{
    public override string Command { get => $"/{DispatcherCommandsEnum.learn_neutral}"; set => base.Command = value; }
    public override Task<string> ExecuteAsync(string? param, Update executionContext, IBotAdapter service)
    {
        return Task.FromResult(param ?? string.Empty);
    }

    public override async Task<string> ExecuteAfterAsync(string? param, Update executionContext, IBotAdapter service, IDispatcher dispatcher)
    {
        var split = param?.Split(' ');
        if(split?.Length != 3)
            return $"Неверные параметры команды. Вводите {Command} айди_схемы количество_магии количество_технологии";
        var dto = new LearnNeutralDto { 
            TgId = BotHelper.GetChatId(executionContext), 
            SearchKey = split[0], 
            MagicPoints = decimal.TryParse(split[1], out decimal result) ? result : 0,
            TechPoints = decimal.TryParse(split[2], out decimal result2) ? result2 : 0
        };
        var response = await dispatcher.DispatchSimple<LearnNeutralDto, string>(DispatcherCommandsEnum.learn_neutral.ToString(), dto);
        return await base.ExecuteAfterAsync(response, executionContext, service, dispatcher);
    }
}
