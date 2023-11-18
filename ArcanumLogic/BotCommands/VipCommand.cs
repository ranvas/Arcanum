using ArcanumLogic.Dtos;
using Integrators.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.Abstractions;
using TgBot.DataSphere;

namespace ArcanumLogic.BotCommands;

public class VipCommand : DefaultCommand
{
    public override string Command { get; set; } = $"/{DispatcherCommandsEnum.login_vip}";
    public override async Task<bool> ExecuteBeforeAsync(string? param, Update executionContext, IBotAdapter service, IDispatcher dispatcher)
    {
        if (!await base.ExecuteBeforeAsync(param, executionContext, service, dispatcher))
            return false;
        var tgId = BotHelper.GetChatId(executionContext);
        var tgName = BotHelper.GetUserName(executionContext);
        var request = new RequestBase() { TgId = tgId, TgName = tgName };
        return await dispatcher.DispatchSimple<RequestBase, bool>(DispatcherCommandsEnum.login_vip.ToString(), request);
    }

    public override Task<string> ExecuteAsync(string? param, Update executionContext, IBotAdapter service)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Вы авторизованы как исследователь");
        sb.AppendLine($"/{DispatcherCommandsEnum.bid.ToString()} - сделать ставку в очках Судьбы на пространство Судьбы:  /bid айди_пространства количество_очков_судьбы");
        sb.AppendLine($"/{DispatcherCommandsEnum.learn.ToString()} - выучить магическую или технологическую схему: /learn айди_схемы");
        sb.AppendLine($"/{DispatcherCommandsEnum.learn_neutral.ToString()} - выучить нейтральную схему: /learn_neutral айди_схемы количество_магии количество_технологии");
        //sb.AppendLine($"/{DispatcherCommandsEnum.transfer_destiny.ToString()}");
        sb.AppendLine($"/{DispatcherCommandsEnum.schemes.ToString()} - посмотреть все свои изученные схемы");
        sb.AppendLine($"/{DispatcherCommandsEnum.imagines.ToString()} - посмотреть список пространств Судьбы и остатков по ним");
        return base.ExecuteAsync(sb.ToString(), executionContext, service);
    }
    
}
