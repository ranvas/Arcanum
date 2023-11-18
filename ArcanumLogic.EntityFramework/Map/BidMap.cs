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
    public class BidMap : IEntityTypeConfiguration<Bid>
    {
        public void Configure(EntityTypeBuilder<Bid> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("bids");

            builder.Property(x => x.Id).HasColumnName("bid_id");
            builder.Property(x => x.Value).HasColumnName("value");
            builder.Property(x => x.AccountId).HasColumnName("account_id");
            builder.Property(x => x.EmagineId).HasColumnName("imagine_id");
            builder.Property(x => x.Payed).HasColumnName("payed");
            builder.Property(x => x.Cycle).HasColumnName("cycle");
            builder.Property(x => x.Currency).HasColumnName("currency");
        }
    }
}
