using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.DataAccess.Persistence.Configurations
{
    public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
    {
        public void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {
            builder.HasIndex(e => e.ApplicationUserId);
            builder.Property(e => e.CreatedOn).HasDefaultValueSql("GETDATE()");
        }
    }
}
