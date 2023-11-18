using ArcanumLogic.EntityFramework;
using ArcanumLogic.Sheets;
using DataAccess.Abstractions;
using GoogleSheet.Abstractions;
using GoogleSheet.Core;
using Integrators.Abstractions;
using Integrators.Core;
using Integrators.Dispatcher;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TgBot.Abstractions;
using TgBot.DataSphere;

namespace ArcanumLogic
{
    public class ArcanumIntegrator : 合気道Integrator
    {
        public ArcanumIntegrator()
        {
        }

        protected override void Init(IServiceCollection services, IConfiguration configuration)
        {
            base.Init(services, configuration);
            var config = new ArcanumOptions();
            configuration.GetSection("ArcanumOptions").Bind(config);
            services.AddSingleton(config);
            services.AddSingleton<ArcanumManager>();
            services.AddSingleton<AccountSheet>();
            services.AddSingleton<GoogleSheetDataManager>();
            services.AddTransient<ImagineSheet>();
            services.AddTransient<AccountSheet>();
            services.AddTransient<TreeSheet>();
            services.AddTransient<FabricaSheet>();

            services.AddSingleton(DispatcherFactory);
            services.AddSingleton(BotFactory);

            services.AddHostedService<DispatchedHostedService>();
            services.AddTransient<ISheetAdapter, SheetAdapter>();
            services.AddTransient<MyContext>();
            services.AddSingleton<IDataAccessConfiguration>((_)=>new DataAccessConfiguration(config.ConnectionString));
            services.AddSingleton<MyDataManager>();
            
        }

        public static IDispatcher DispatcherFactory(IServiceProvider provider)
        {
            var dispatcher = new ArcanumDispatcher(provider);
            return dispatcher;
        }

        public static ArcanumBot BotFactory(IServiceProvider provider)
        {
            var dispatcher = provider.GetRequiredService<IDispatcher>();
            var config = provider.GetRequiredService<ArcanumOptions>();

            var bot = new ArcanumBot(config.BotToken, new RootParams() { Logs = new List<long> { config.ChatForLogs }, Roots = config.Roots }, dispatcher);
            return bot;
        }
    }
}
