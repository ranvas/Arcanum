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
    public class ImagineMap : IEntityTypeConfiguration<Imagine>
    {
        public void Configure(EntityTypeBuilder<Imagine> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("imagines");

            builder.Property(x => x.Id).HasColumnName("imagine_id");
            builder.Property(x => x.SearchKey).HasColumnName("search_key");
            builder.Property(x => x.Description).HasColumnName("description");
            builder.Property(x => x.Currency).HasColumnName("currency");
            builder.Property(x => x.ValueStart).HasColumnName("value_start");
            builder.Property(x => x.MagicValue).HasColumnName("magic_value");

            builder.Ignore(x => x.Bids);
            builder.Ignore(x => x.Capacity);
        }
    }
}
