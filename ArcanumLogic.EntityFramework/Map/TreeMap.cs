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
    public class TreeMap : IEntityTypeConfiguration<Tree>
    {
        public void Configure(EntityTypeBuilder<Tree> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("trees");

            builder.Property(x => x.Id).HasColumnName("tree_id");
            builder.Property(x => x.Name).HasColumnName("name");
            builder.Property(x => x.SearchKey).HasColumnName("search_key");
            builder.Property(x => x.Currency).HasColumnName("currency");
            builder.Property(x => x.Cost).HasColumnName("cost");
            builder.Property(x => x.ParentsTree).HasColumnName("parent_tree");
            builder.HasMany(x => x.Researches)
                .WithOne(t => t.Tree)
                .HasForeignKey(r => r.SearchKey)
                .HasPrincipalKey(t => t.SearchKey);

            builder.Ignore(x => x.CostValue);
            builder.Ignore(x => x.Requirements);
            //builder.Ignore(x => x.Researches);
        }
    }
}
