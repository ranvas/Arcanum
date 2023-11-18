using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanumLogic.EntityFramework.Model
{
    public class Tree : ArcanumLogicBase
    {
        public List<Research> Researches { get; set; } = new();
        [Display(Name = "название схемы")]
        public string Name { get; set; } = string.Empty;
        [Display(Name = "айди схемы")]
        public string SearchKey { get; set; } = string.Empty;
        [Display(Name = "тип схемы")]
        public string Currency { get; set; } = string.Empty;
        [Display(Name = "стоимость")]
        public string Cost { get; set; } = string.Empty;
        public decimal CostValue
        {
            get
            {
                return decimal.TryParse(Cost, out decimal value) ? value : 0;
            }
        }

        [Display(Name = "родительская схема")]
        public string ParentsTree { get; set; } = string.Empty;

        public List<string> Requirements
        {
            get
            {
                return string.IsNullOrEmpty(ParentsTree) ? new() : ParentsTree.Split(';').ToList();
            }
        }
    }
}
