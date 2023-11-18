using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanumLogic.Dtos
{
    public class TransferDestinyDto
    {
        public long TgFrom { get; set; }
        public int Count { get; set; }
        public string TransferTo { get; set; } = string.Empty;
    }
}
