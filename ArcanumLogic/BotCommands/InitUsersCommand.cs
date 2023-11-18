using ArcanumLogic.Dtos;
using Integrators.Abstractions;
using Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.Abstractions;
using TgBot.DataSphere;

namespace ArcanumLogic.BotCommands;

public class InitUsersCommand : RootSendCommand
{
    public override string Command { get; set; } = $"/{DispatcherCommandsEnum.init_users}";

    public override Task<string> ExecuteAsync(string? param, Update executionContext, IBotAdapter service)
    {
        var message = $"Вы уверены? Старые пользователи будут утеряны, включая все ставки и переводы. Введите /{DispatcherCommandsEnum.init_users_sure_yes}";
        return Task.FromResult(message);
    }

}
