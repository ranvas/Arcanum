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

public class InitImagineCommand : RootSendCommand
{
    public override string Command { get; set; } = $"/{DispatcherCommandsEnum.init_imagine}";
    public override Task<string> ExecuteAsync(string? param, Update executionContext, IBotAdapter service)
    {
        var message = $"Вы уверены? Старые пространства будут утеряны, включая все ставки. Введите /{DispatcherCommandsEnum.init_imagine_sure_yes}";
        return Task.FromResult(message);
    }
}
