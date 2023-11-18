using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanumLogic.EntityFramework.Model
{
    public class Transfer : ArcanumLogicBase
    {
        public long AccountFromId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public string TransferTime { get; set; } = string.Empty; 
        public decimal CurrencyValue { get; set; }
    }
}
