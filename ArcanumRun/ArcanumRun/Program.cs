// See https://aka.ms/new-console-template for more information
using ArcanumLogic;
using Integrators.Abstractions;
using Integrators.Core;
using Integrators.Dispatcher;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TgBot.DataSphere;

Console.WriteLine("Hello, World!");
var integrator = new HSDIntegrator<ArcanumDispatcher, MyOwnBot>(HostDispatcherFactory, BotFactory, "bot_start");
try
{
    await integrator.RunDefaultIntegrator(args);
}
catch (Exception e)
{
    Console.WriteLine(e.ToString());
}

static IDispatcher HostDispatcherFactory(IServiceProvider provider)
{
    var dispatcher = new ArcanumDispatcher(provider);
    dispatcher.RegisterService<MyOwnBot>("bot_start", "StartReceiving");
    return dispatcher;
}

static MyOwnBot BotFactory(IServiceProvider provider)
{
    var bot = new MyOwnBot("6636861811:AAE_Kfx_ZbbUdWLV-FmWXxR1NvQ0c2wBFAM");
    return bot;
}
Console.WriteLine("in the end");