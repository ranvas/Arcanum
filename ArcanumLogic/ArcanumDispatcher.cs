using ArcanumLogic.Dtos;
using Integrators.Abstractions;
using Integrators.Core;
using Integrators.Dispatcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TgBot.DataSphere;

namespace ArcanumLogic
{
    public class ArcanumDispatcher : HostDispatcher
    {
        public ArcanumDispatcher(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            RegisterDispatcher();
        }
        public void RegisterDispatcher()
        {
            RegisterService<ArcanumBot>(DispatchedHostedService.StartKey, nameof(ArcanumBot.StartAsync));
            RegisterService<ArcanumBot>(DispatchedHostedService.StopKey, nameof(ArcanumBot.StopAsync));
            RegisterService<string, ArcanumBot>($"log", nameof(BotDispatchingServiceBase.LogMessage));
            RegisterService<string, ArcanumBot>($"debug", nameof(BotDispatchingServiceBase.LogMessage));
            RegisterService<ArcanumManager>(DispatcherCommandsEnum.init_users.ToString(), nameof(ArcanumManager.InitUsers));
            RegisterService<RequestBase, ArcanumManager>(DispatcherCommandsEnum.start.ToString(), nameof(ArcanumManager.Login));
            RegisterService<RequestBase, ArcanumManager>(DispatcherCommandsEnum.status.ToString(), nameof(ArcanumManager.GetFinalStatus));
            RegisterService<ArcanumManager>(DispatcherCommandsEnum.init_command.ToString(), nameof(ArcanumManager.InitCommand));
            RegisterService<ArcanumManager>(DispatcherCommandsEnum.login_vip.ToString(), nameof (ArcanumManager.LoginVip));
            RegisterService<BidDto, ArcanumManager>(DispatcherCommandsEnum.bid.ToString(), nameof(ArcanumManager.MakeADeal));
            RegisterService<string, ArcanumManager>(DispatcherCommandsEnum.start_cycle.ToString(), nameof(ArcanumManager.Startycle));
            RegisterService<ArcanumManager>(DispatcherCommandsEnum.stop_cycle.ToString(), nameof(ArcanumManager.StopCycle));
            RegisterService<ArcanumManager>(DispatcherCommandsEnum.calculate.ToString(), nameof(ArcanumManager.Calculate));
            RegisterService<long, ArcanumManager>(DispatcherCommandsEnum.schemes.ToString(), nameof(ArcanumManager.GetSchemas));
            RegisterService<LearnDto, ArcanumManager>(DispatcherCommandsEnum.learn.ToString(), nameof(ArcanumManager.Learn));
            RegisterService<LearnNeutralDto, ArcanumManager>(DispatcherCommandsEnum.learn_neutral.ToString(), nameof(ArcanumManager.LearnNeutral));
            RegisterService<ArcanumManager>(DispatcherCommandsEnum.imagines.ToString(), nameof(ArcanumManager.GetImagines));
            RegisterService<ArcanumManager>(DispatcherCommandsEnum.update_users.ToString(), nameof(ArcanumManager.UpdateUsers));
            RegisterService<ArcanumManager>(DispatcherCommandsEnum.admin_all_schemes.ToString(), nameof(ArcanumManager.AdminAllSchemes));
            RegisterService<TransferDestinyDto, ArcanumManager>(DispatcherCommandsEnum.transfer_destiny.ToString(), nameof(ArcanumManager.TransferDestiny));
            RegisterService<string, ArcanumBot>("public_log", nameof(ArcanumBot.PublicLog));
        }
    }
}
