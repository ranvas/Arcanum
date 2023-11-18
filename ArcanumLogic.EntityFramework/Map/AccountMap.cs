using ArcanumLogic.EntityFramework.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanumLogic.EntityFramework.Map
{
    public class AccountMap: IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("accounts");

            builder.Property(x => x.Id).HasColumnName("account_id");
            builder.Property(x => x.TgId).HasColumnName("telegram_id");
            builder.Property(x => x.Name).HasColumnName("character_name");
            builder.Property(x => x.TgName).HasColumnName("telegram_name");
            builder.Property(x => x.WalletCode).HasColumnName("wallet_code");
            builder.Property(x => x.DestinyPointsStart).HasColumnName("destiny_points_start");
            builder.Property(x => x.MagicPointsStart).HasColumnName("magic_points_start");
            builder.Property(x => x.TechPointsStart).HasColumnName("tech_points_start");
            builder.Property(x => x.IsVIP).HasColumnName("is_vip");
            builder.Property(x => x.ZonesText).HasColumnName("zones_text");
            builder.Property(x => x.Scoring).HasColumnName("scoring");
            builder.Property(x => x.Fabrica).HasColumnName("fabrica_searchkey");
            builder.Property(x => x.Experience).HasColumnName("exp");

            builder.Ignore(x => x.IsVIPValue);
            builder.Ignore(x => x.Transfers);
            builder.Ignore(x => x.Bids);
            builder.Ignore(x => x.ScoringValue);
        }
    }
}
