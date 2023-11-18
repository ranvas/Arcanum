using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanumLogic
{
    public class ArcanumOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string GoogleSpreadSheetId { get; set; } = string.Empty;
        public string BotToken { get; set; } = string.Empty;
        public long ChatForLogs { get; set; }
        public int StartCycle { get; set; }
        public List<long> Roots { get; set; } = new();
        public double Amagic { get; set; }
        public double Bmagic { get; set;}
        public decimal Cmagic { get; set; }
    }
}
