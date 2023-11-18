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
    public class ResearchMap : IEntityTypeConfiguration<Research>
    {
        public void Configure(EntityTypeBuilder<Research> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("research");

            builder.Property(x => x.Id).HasColumnName("research_id");
            builder.Property(x => x.AccountId).HasColumnName("account_id");
            builder.Property(x => x.SearchKey).HasColumnName("search_key");
            builder.Property(x => x.TimeOfResearch).HasColumnName("time_of_research");
            builder.HasOne(x => x.Tree)
                .WithMany(t => t.Researches)
                .HasForeignKey(r => r.SearchKey)
                .HasPrincipalKey(t => t.SearchKey);


            //builder.Ignore(x => x.Tree);
        }
    }
}
