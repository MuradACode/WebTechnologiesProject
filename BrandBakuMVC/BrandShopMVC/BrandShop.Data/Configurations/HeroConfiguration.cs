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
    public class HeroConfiguration : IEntityTypeConfiguration<Hero>
    {
        public void Configure(EntityTypeBuilder<Hero> builder)
        {
            builder.Property(x => x.Title).HasMaxLength(50);
            builder.Property(x => x.Description).HasMaxLength(200);
            builder.Property(x => x.BtnText).HasMaxLength(30);
            builder.Property(x => x.BtnLink).HasMaxLength(300);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(x => x.Image).HasMaxLength(100);
            builder.Property(x => x.BackgroundImage).HasMaxLength(100);
            builder.Ignore(x => x.ImageFile);
            builder.Ignore(x => x.BgImageFile);
        }
    }
}
