using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanumLogic.EntityFramework.Model
{
    public class Imagine : ArcanumLogicBase
    {
        [Display(Name = "id пространства")]
        public string SearchKey { get; set; } = string.Empty;
        [Display(Name = "название")]
        public string Description { get; set; } = string.Empty;
        [Display(Name = "ресурс")]
        public string Currency { get; set; } = string.Empty;
        [Display(Name = "стартовая выработка")]
        public string ValueStart { get; set; } = string.Empty;
        [Display(Name = "модификатор")]
        public string MagicValue { get; set; } = string.Empty;
        public List<Bid> Bids { get; set; } = new();
        public int SumBidsByCycle(int cycle) => Bids.Where(b => b.Cycle == cycle).Sum(b => b.Value);
        public decimal Capacity => decimal.TryParse(MagicValue, out var mv) && decimal.TryParse(ValueStart, out var vs) ? mv * vs : 0;
    }
}
