using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanumLogic.EntityFramework.Model
{
    public class Bid : ArcanumLogicBase
    {
        public int Value { get; set; }
        public long AccountId { get; set; }
        public long EmagineId { get; set; }
        public decimal Payed { get; set; }
        public int Cycle { get; set; }
        public string Currency { get; set; } = string.Empty;
    }
}
