using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.DataAccess.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasIndex(e => e.Title).IsUnique();


            builder.Property(e => e.Title).HasMaxLength(500);
            builder.Property(e => e.Description).HasMaxLength(500);
            builder.Property(e => e.Summary).HasMaxLength(250);
            builder.Property(e => e.CreatedOn).HasDefaultValueSql("GETDATE()");
        }
    }
}
