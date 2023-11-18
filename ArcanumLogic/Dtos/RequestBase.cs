using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanumLogic.Dtos
{
    public class RequestBase
    {
        public long TgId { get; set; }
        public string TgName { get; set; } = string.Empty;
    }
}
