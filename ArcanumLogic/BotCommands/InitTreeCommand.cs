using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.Abstractions;
using TgBot.DataSphere;

namespace ArcanumLogic.BotCommands;

public class InitTreeCommand : InitUsersCommand
{
    public override string Command { get; set; } = $"/{DispatcherCommandsEnum.init_tree}";

    public override Task<string> ExecuteAsync(string? param, Update executionContext, IBotAdapter service)
    {
        var message = $"Вы уверены? Все исследования будут утеряны. Введите /{DispatcherCommandsEnum.init_tree_sure_yes}";
        return Task.FromResult(message);
    }
}
