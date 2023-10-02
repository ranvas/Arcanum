using ArcanumLogic.Bot;
using Integrators.Abstractions;
using Integrators.Dispatcher;
using TgBot.DataSphere;

namespace ArcanumLogic
{
    public class ArcanumDispatcher : HostDispatcher
    {
        public ArcanumDispatcher(IServiceProvider provider) : base(provider)
        {

            RegisterMainCommands();
        }

        private void RegisterMainCommands()
        {
            RegisterService<MyOwnBot>("default", typeof(string), "LogMessage");
            RegisterService<long, ArcanumManager>(DispatcherCommandsEnum.status.ToString(), nameof(ArcanumManager.GetStatus));
        }

    }
}