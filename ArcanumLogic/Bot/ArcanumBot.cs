using ArcanumLogic.Dtos;
using Integrators.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBot.DataSphere;

namespace ArcanumLogic.Bot
{
    public class ArcanumBot : MyOwnBot
    {
        public ArcanumBot(string token, long logs, List<long> roots, IDispatcher dispatcher) : base(token, logs, roots, "ArcanumBase", dispatcher)
        {
            AddCommand(new GetStatusCommand());
        }

        protected override Task<bool> HandleCommandBefore(string command, string? param, Update executionContext)
        {
            return base.HandleCommandBefore(command, param, executionContext);
        }

        public async Task<bool> IsEasyDispatching(ArcanumBotCommandsEnum command)
        {
            try
            {
                await Dispatcher.DispatchSimple(command.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("easy dispatch debug");
                return false;
            }
            return true;
        }
    }
}
