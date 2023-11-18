﻿using Integrators.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.Abstractions;
using TgBot.DataSphere;

namespace ArcanumLogic.BotCommands;

public class CalculateCommand : RootSendCommand
{
    public override string Command { get; set; } = $"/{DispatcherCommandsEnum.calculate}";
    public override async Task<string> ExecuteAfterAsync(string? param, Update executionContext, IBotAdapter service, IDispatcher dispatcher)
    {
        var result = await dispatcher.DispatchSimple<string>(DispatcherCommandsEnum.calculate.ToString());
        return await base.ExecuteAfterAsync(result, executionContext, service, dispatcher);
    }
}
