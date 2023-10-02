using ArcanumLogic.Bot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanumLogic.Dtos
{
    public class GetStatusResponse
    {
        public string Command { get; set; } = DispatcherCommandsEnum.status.ToString();
        public string ResultMessage { get; set; } = string.Empty;
        public long Id { get; set; }
    }
}
