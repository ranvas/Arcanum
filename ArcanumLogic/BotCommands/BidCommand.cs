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

namespace ArcanumLogic.BotCommands
{
    public class BidCommand : VipCommand
    {
        public override string Command { get => $"/{DispatcherCommandsEnum.bid}"; set => base.Command = value; }

        public override Task<string> ExecuteAsync(string? param, Update executionContext, IBotAdapter service)
        {
            return Task.FromResult(param ?? string.Empty);
        }

        public override async Task<string> ExecuteAfterAsync(string? param, Update executionContext, IBotAdapter service, IDispatcher dispatcher)
        {

            var parts = param?.Split(' ');
            if (parts?.Length != 2)
                return $"Неверные параметры, {Command} айди_пространства количество_очков_судьбы";

            var tgId = BotHelper.GetChatId(executionContext);
            var request = new BidDto { ID = parts![0], SUM = parts![1], TgId = tgId };
            var result = await dispatcher.DispatchSimple<BidDto, string>(DispatcherCommandsEnum.bid.ToString(), request) ?? string.Empty;
            return await base.ExecuteAfterAsync(result, executionContext, service, dispatcher);
        }
    }
}
