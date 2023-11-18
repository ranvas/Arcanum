using ArcanumLogic.EntityFramework.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanumLogic.EntityFramework.Map
{
    public class TransferMap : IEntityTypeConfiguration<Transfer>
    {
        public void Configure(EntityTypeBuilder<Transfer> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("transfers");

            builder.Property(x => x.Id).HasColumnName("transfer_id");
            builder.Property(x => x.AccountFromId).HasColumnName("account_id");
            builder.Property(x => x.Comment).HasColumnName("comment");
            builder.Property(x => x.Currency).HasColumnName("currency");
            builder.Property(x => x.TransferTime).HasColumnName("transfer_time");
            builder.Property(x => x.CurrencyValue).HasColumnName("currency_value");
        }
    }
}
