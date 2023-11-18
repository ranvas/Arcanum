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
    public class TransferDestinyCommand : VipCommand
    {
        public override string Command { get => $"/{DispatcherCommandsEnum.transfer_destiny}"; set => base.Command = value; }

        public override Task<string> ExecuteAsync(string? param, Update executionContext, IBotAdapter service)
        {
            return Task.FromResult(param ?? string.Empty);
        }

        public override async Task<string> ExecuteAfterAsync(string? param, Update executionContext, IBotAdapter service, IDispatcher dispatcher)
        {
            var dto = new TransferDestinyDto { TgFrom = BotHelper.GetChatId(executionContext) };
            var split = param?.Split(' ');
            if (split?.Length != 2)
                return $"Неверные параметры, вводите {Command} кошелек_кому сколько_переводить";
            dto.Count = int.TryParse(split[1], out int count) ? count : 0;
            dto.TransferTo = split[0];
            var result = await dispatcher.DispatchSimple<TransferDestinyDto, string>( DispatcherCommandsEnum.transfer_destiny.ToString(), dto);
            return await base.ExecuteAfterAsync(result, executionContext, service, dispatcher);
        }
    }
}
