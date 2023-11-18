using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanumLogic.Dtos
{
    public class ResponseBase
    {
        public string ResultMessage { get; set; } = string.Empty;
        public long Id { get; set; }
        public bool IsError { get; set; }
    }
}
