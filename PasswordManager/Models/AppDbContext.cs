using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PasswordManager.Models
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MyPasswords>(x => x.HasKey(t => t.Id));
            builder.Entity<MyPasswords>(x => x.Property(t => t.Id).UseIdentityColumn());
            builder.Entity<MyPasswords>(x => x.Property(t => t.Description).HasMaxLength(100).IsRequired());
            builder.Entity<MyPasswords>(x => x.Property(t => t.URL).HasMaxLength(250).IsRequired());
            builder.Entity<MyPasswords>(x => x.Property(t => t.UserName).HasMaxLength(25).IsRequired());
            builder.Entity<MyPasswords>(x => x.Property(t => t.Password).HasMaxLength(50).IsRequired());

            builder.Entity<Categories>(x => x.HasKey(t => t.Id));
            builder.Entity<Categories>(x => x.Property(t => t.Id).UseIdentityColumn());
            builder.Entity<Categories>(x => x.Property(t => t.CategoryName).HasMaxLength(25).IsRequired());

            builder.Entity<Categories>().HasMany(x => x.MyPasswords).WithOne(x => x.Categories).HasForeignKey(x => x.CategoryID);
            builder.Entity<AppUser>().HasMany(x => x.MyPasswords).WithOne(x => x.AppUser).HasForeignKey(x => x.UserID);
            builder.Entity<AppUser>().HasMany(x => x.Categories).WithOne(x => x.AppUser).HasForeignKey(x => x.UserID);

            base.OnModelCreating(builder);
        }

        public DbSet<MyPasswords> MyPasswords { get; set; }
        public DbSet<Categories> Categories { get; set; }

        public override int SaveChanges()
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity baseEntity)
                {
                    switch (item.Entity)
                    {
                        case EntityState.Added:
                            {
                                baseEntity.CreatedDate = DateTime.Now;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                baseEntity.UpdatedDate = DateTime.Now;
                                break;
                            }
                    }
                }

            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity entityReference)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            {
                                entityReference.CreatedDate = DateTime.Now;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                Entry(entityReference).Property(x => x.CreatedDate).IsModified = false;

                                entityReference.UpdatedDate = DateTime.Now;
                                break;
                            }
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
