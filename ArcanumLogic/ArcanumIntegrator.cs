using ArcanumLogic.Bot;
using Integrators.Abstractions;
using Integrators.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanumLogic
{
    public class ArcanumIntegrator : HSDIntegrator<ArcanumDispatcher, ArcanumBot>
    {
        public ArcanumIntegrator() : 
            base(HostDispatcherFactory, BotFactory, "bot_start")
        {
        }

        protected override void Init(IServiceCollection services, IConfiguration configuration)
        {
            base.Init(services, configuration);
            services.AddSingleton<ArcanumManager>();
        }

        public static IDispatcher HostDispatcherFactory(IServiceProvider provider)
        {
            var dispatcher = new ArcanumDispatcher(provider);
            dispatcher.RegisterService<ArcanumBot>("bot_start", "StartReceiving");
            return dispatcher;
        }

        public static ArcanumBot BotFactory(IServiceProvider provider)
        {
            var dispatcher = new ArcanumDispatcher(provider);
            var bot = new ArcanumBot("6363533444:AAFd_ONV7tK67ZCggkWchu5GXO5eEdQejeg", -1001968103278, new List<long> { 50789630 }, dispatcher);
            return bot;
        }
    }
}
