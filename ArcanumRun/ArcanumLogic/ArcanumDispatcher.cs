using Integrators.Dispatcher;

namespace ArcanumLogic
{
    public class ArcanumDispatcher : HostDispatcher
    {
        public ArcanumDispatcher(IServiceProvider provider) : base(provider)
        {
            

        }

        private void RegisterMainCommands()
        {
            RegisterService<ArcanumManager>("manager", "Instance");
        }

    }
}