using ArcanumLogic.Dtos;
using Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using TgBot.DataSphere;

namespace ArcanumLogic.Bot
{
    public class GetStatusCommand : DefaultCommand
    {
        public override string Command { get; set; } = $"/{ArcanumBotCommandsEnum.status}";

        public override async Task<string> ExecuteDispatched(string? param, Update executionContext, BotDispatchingServiceBase service)
        {
            var tgId = BotHelper.GetChatId(executionContext);
            var model = await service.Dispatcher.DispatchSimple<long, GetStatusResponse>(DispatcherCommandsEnum.status.ToString(), tgId);
            var message = $"``` {Serializer.ToJSON(model)} ```";
            await service.SendTextMessage(tgId, message, Telegram.Bot.Types.Enums.ParseMode.Markdown);
            return message;
        }
    }
}
