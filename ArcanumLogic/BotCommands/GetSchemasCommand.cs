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
    public class GetSchemasCommand : VipCommand
    {
        public override string Command { get => $"/{DispatcherCommandsEnum.schemes}"; set => base.Command = value; }

        public override Task<string> ExecuteAsync(string? param, Update executionContext, IBotAdapter service)
        {
            return Task.FromResult(param ?? string.Empty);
        }

        public override async Task<string> ExecuteAfterAsync(string? param, Update executionContext, IBotAdapter service, IDispatcher dispatcher)
        {
            var tgId = BotHelper.GetChatId(executionContext);
            var result = await dispatcher.DispatchSimple<long, string>(DispatcherCommandsEnum.schemes.ToString(), tgId);
            return await base.ExecuteAfterAsync(result, executionContext, service, dispatcher);
        }
    }
}
