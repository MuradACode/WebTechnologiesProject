using BrandShop.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandShop.Data.Configurations
{
    public class DiscountedConfiguration : IEntityTypeConfiguration<Discounted>
    {
        public void Configure(EntityTypeBuilder<Discounted> builder)
        {
            builder.Property(x => x.Title).HasMaxLength(30);
            builder.Property(x => x.StartDate).HasMaxLength(50);
            builder.Property(x => x.EndDate).HasMaxLength(50);
            builder.Property(x => x.BtnText).HasMaxLength(30);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
