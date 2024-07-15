using System.Reflection;
using Furni.DataAccess.Persistence.Seeds;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Furni.DataAccess.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Apply Configurations from all IEntityTypeConfiguration<T> that contain Fluent Api
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

			builder.Entity<Review>(entity =>
			{
				entity.Property(e => e.Rating)
				  .IsRequired()
				  .HasDefaultValue(1)  // Optional: Set a default value if needed
				  .HasColumnName("Rating")
				  .HasColumnType("int");
				entity.HasCheckConstraint("CK_Rating_Range", "[Rating] >= 1 AND [Rating] <= 5");
			});

			// Seed Categories Data 
			builder.Entity<Category>().HasData(CategoriesData.Seed());

            var cascadeFKs = builder.Model.GetEntityTypes()
                                          .SelectMany(t => t.GetForeignKeys())
                                          .Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade && !fk.IsOwnership);
            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            base.OnModelCreating(builder);
        }

		public DbSet<Category> Categories { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderDetail> OrderDetails { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductImage> ProductImages { get; set; }
		public DbSet<Review> Reviews { get; set; }
		public DbSet<ShoppingCart> ShoppingCarts { get; set; }

	}
}
