using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanumLogic.EntityFramework.Model
{
    public class Research : ArcanumLogicBase
    {
        public long? AccountId { get; set; }
        public string SearchKey { get; set; } = string.Empty;
        public string TimeOfResearch { get; set; } = string.Empty;
        public Tree? Tree { get; set; }
    }
}
