using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanumLogic.Dtos
{
    public class LearnNeutralDto: LearnDto
    {
        public decimal MagicPoints { get; set; }
        public decimal TechPoints { get; set; }
    }
}
