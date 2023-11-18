using ArcanumLogic.EntityFramework.Model;
using Integrators.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanumLogic
{
    public static class ArcanumHelper
    {
        public static Task LogMessage(string message, IDispatcher dispatcher)
        {
            try
            {
                return dispatcher.DispatchSimple(message, "public_log");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return Task.CompletedTask;
        }

        public static string GetDateTimeString()
        {
            return DateTimeOffset.Now.ToString("ddMMyy HH:mm:ss");
        }
    }
}
