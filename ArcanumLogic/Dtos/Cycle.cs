using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanumLogic.Dtos
{
    public class Cycle
    {
        public int CurrentCycle { get; set; }
        public bool Started { get; set; }
        public bool Locked { get; set; } 
        public Cycle() : this(0) { }
        public Cycle(int current) { CurrentCycle = current; }
    }
}
