using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanumLogic.Dtos
{
    public class BidDto
    {
        public long TgId { get; set; } = 0;
        public string ID { get; set; } = string.Empty;
        public string SUM { get; set; } = string.Empty;
        public int SumValue
        {
            get
            {
                return int.TryParse(SUM, out var sumValue) ? sumValue : 0;

            }
            set
            {
                SUM = value.ToString();
            }
        }
    }
}
