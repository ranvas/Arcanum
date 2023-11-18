using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ArcanumLogic.EntityFramework.Model
{
    [Display(Name = "Пользователь")]
    public class Account : ArcanumLogicBase
    {
        public long TgId { get; set; }

        [Display(Name = "имя персонажа")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "TgName")]
        public string TgName { get; set; } = string.Empty;

        [Display(Name = "Код счета")]
        public string? WalletCode { get; set; }

        [Display(Name = "Стартовый баланс очков судьбы")]
        public string DestinyPointsStart { get; set; } = string.Empty;
        public int DestinyPointsStartValue() =>
            int.TryParse(DestinyPointsStart, out int result) ? result : 0;

        [Display(Name = "Стартовый баланс магии")]
        public string MagicPointsStart { get; set; } = string.Empty;
        public int MagicPointsStartValue() =>
            int.TryParse(MagicPointsStart, out int result) ? result : 0;

        [Display(Name = "Стартовый баланс технологии")]
        public string TechPointsStart { get; set; } = string.Empty;
        public int TechPointsStartValue() =>
            int.TryParse(TechPointsStart, out int result) ? result : 0;

        [Display(Name = "Видит исследования")]
        public string IsVIP { get; set; } = string.Empty;

        [Display(Name = "Зоны")]
        public string ZonesText { get; set; } = string.Empty;

        [Display(Name = "Стартовый скоринг")]
        public string Scoring { get; set; } = string.Empty;
        public decimal ScoringValue
        {
            get
            {
                return decimal.TryParse(Scoring, out decimal result) ? result : 0;
            }
            set
            {
                Scoring  = value.ToString();
            }
        }

        [Display(Name = "айди фабрики")]
        public string Fabrica { get; set; } = string.Empty;
        public bool IsVIPValue { get => this.IsVIP.ToLower() == "true" ? true : false; }
        public decimal Experience { get; set; }
        public List<Transfer> Transfers { get; set; } = new();
        public List<Bid> Bids { get; set; } = new();
        public decimal DestinySum() =>
            DestinyPointsStartValue() + Transfers.Where(t => t.Currency == CurrencyEnum.destiny.ToString()).Sum(t => t.CurrencyValue);

        public decimal MagicSum() =>
            MagicPointsStartValue() + Transfers.Where(t => t.Currency == CurrencyEnum.magic.ToString()).Sum(t => t.CurrencyValue);

        public decimal TechSum() =>
            TechPointsStartValue() + Transfers.Where(t => t.Currency == CurrencyEnum.tech.ToString()).Sum(t => t.CurrencyValue);

        public decimal AccountSumBids(long imagineId, int cycle) => Bids.Where(b=>b.EmagineId == imagineId && b.Cycle == cycle).Sum(b => b.Value);

        public decimal SumBidsMagic(int cycle) => Bids.Where(b => b.Cycle == cycle &&  b.Currency == CurrencyEnum.magic.ToString()).Sum(b => b.Value);
        public decimal SumBidsTech(int cycle) => Bids.Where(b => b.Cycle == cycle && b.Currency == CurrencyEnum.tech.ToString()).Sum(b => b.Value);
        public decimal SumBidsNeutral(int cycle) => Bids.Where(b => b.Cycle == cycle && b.Currency == CurrencyEnum.neutral.ToString()).Sum(b => b.Value);

    }
}
