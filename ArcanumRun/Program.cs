// See https://aka.ms/new-console-template for more information
using ArcanumLogic;
using ArcanumLogic.Bot;
using Integrators.Abstractions;
using Integrators.Core;
using Integrators.Dispatcher;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TgBot.DataSphere;

Console.WriteLine("Hello, World!");
var integrator = new ArcanumIntegrator();
try
{
    await integrator.RunDefaultIntegrator(args);
}
catch (Exception e)
{
    Console.WriteLine(e.ToString());
}


Console.WriteLine("in the end");