using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ArcanumLogic.EntityFramework.Model
{
    public class Fabrica : ArcanumLogicBase
    {
        [Display(Name = "айди фабрики")]
        public string SearchKey { get; set; } = string.Empty;
        [Display(Name = "фабрика")]
        public string Name { get; set; } = string.Empty;
        [Display(Name = "текущий максимум выработки")]
        public string CurrentMax { get; set; } = string.Empty;
        public int CMValue
        {
            get { return int.TryParse(CurrentMax, out int result) ? result : 0; }
            set { CurrentMax = value.ToString(); }
        }
        [Display(Name = "выработка с 1ого спутника")]
        public string ProgressPerOne { get; set; } = string.Empty;
        public int PPOValue
        {
            get { return int.TryParse(ProgressPerOne, out int result) ? result : 0; }
            set { ProgressPerOne = value.ToString(); }
        }
    }
}
