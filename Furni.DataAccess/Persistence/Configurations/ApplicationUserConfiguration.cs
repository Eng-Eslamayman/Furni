using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.DataAccess.Persistence.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasIndex(e => e.Email).IsUnique();
            builder.HasIndex(e => e.UserName).IsUnique();

            builder.Property(e => e.FullName).HasMaxLength(100);
            builder.Property(e => e.CreatedOn).HasDefaultValueSql("GETDATE()");
        }
    }
}
