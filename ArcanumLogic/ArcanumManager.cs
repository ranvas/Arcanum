using ArcanumLogic.Bot;
using ArcanumLogic.Dtos;
using Integrators.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanumLogic
{
    public class ArcanumManager
    {
        private readonly IDispatcher _dispatcher;
        public ArcanumManager(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;

        }

        public ArcanumManager Instance() { return this; }

        public Task<GetStatusResponse> GetStatus(long tgId)
        {
            var response = new GetStatusResponse { Id = tgId };
            response.ResultMessage = "Вас покрывает магия SQL и Excel";
            return Task.FromResult(response);
        }

    }
}
