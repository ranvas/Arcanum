using ArcanumLogic.BotCommands;
using ArcanumLogic.Dtos;
using Integrators.Abstractions;
using Integrators.Core;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.DataSphere;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ArcanumLogic
{
    public class ArcanumBot : BotDispatchingServiceBase
    {
        private long PublicId { get; set; }
        public ArcanumBot(string token, RootParams config, IDispatcher dispatcher) : base(token, dispatcher, "ArcanumBot", config)
        {
            PublicId = -1002042572210;
            AddCommand(new LoginCommand());
            AddCommand(new VipCommand());
            AddCommand(new GetStatusCommand());
            AddCommand(new BidCommand());
            AddCommand(new LearnCommand());
            AddCommand(new LearnNeutralCommand());
            AddCommand(new GetSchemasCommand());
            AddCommand(new GetImaginesCommand());
            AddCommand(new TransferDestinyCommand());

            AddCommand(new InitUsersCommand());
            AddCommand(new InitUsersSureYesCommand());
            AddCommand(new InitImagineCommand());
            AddCommand(new InitImagineSureYesCommand());
            AddCommand(new InitTreeCommand());
            AddCommand(new InitTreeSureYesCommand());
            AddCommand(new InitAllCommand());
            AddCommand(new InitFabricasCommand());
            AddCommand(new InitFabricasCommand());

            AddCommand(new StartCycleCommand());
            AddCommand(new StopCycleCommand());
            AddCommand(new CalculateCommand());

            AddCommand(new UpdateImagineCommand());
            AddCommand(new UpdateUsersCommand());
            AddCommand(new AdminAllSchemesCommand());
        }

        public async Task PublicLog(string message)
        {
            await SendTextMessage(PublicId, message);
        }

        public async Task StartAsync()
        {
            //await Dispatcher.DispatchSimple("bot starting", "log");
            StartReceiving();
        }

        public async Task StopAsync()
        {
            await Dispatcher.DispatchSimple("bot stoped", "log");
        }
    }
}
