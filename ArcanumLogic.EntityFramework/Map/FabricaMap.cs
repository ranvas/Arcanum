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
    public class FabricaMap : IEntityTypeConfiguration<Fabrica>
    {
        public void Configure(EntityTypeBuilder<Fabrica> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("fabricas");

            builder.Property(x => x.Id).HasColumnName("fabrica_id");
            builder.Property(x => x.SearchKey).HasColumnName("search_key");
            builder.Property(x => x.Name).HasColumnName("name");
            builder.Property(x => x.CurrentMax).HasColumnName("current_max");
            builder.Property(x => x.ProgressPerOne).HasColumnName("progress_per_one");
            builder.Ignore(x=>x.CMValue);
            builder.Ignore(x => x.PPOValue);
        }
    }
}
