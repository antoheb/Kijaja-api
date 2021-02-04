using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Ad> Ads { get; set; }

        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Ad>()
            .HasOne(x => x.AppUser)
            .WithMany(x => x.UserAds)
            .HasForeignKey(x => x.AppUserId);

            builder.Entity<AppUser>()
            .HasOne(x => x.Address)
            .WithOne(x => x.AppUser)
            .HasForeignKey<Address>(x => x.AppUserId);
        }
    }
}