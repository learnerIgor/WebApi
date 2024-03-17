using Common.Domain;
using Microsoft.EntityFrameworkCore;

namespace Common.Repositories;

public class ApplicationDbContext : DbContext
{
    public DbSet<ToDo> ToDos { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
    public DbSet<ApplicationUserApplicationRole> ApplicationUserApplicationRole {  get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ToDo>().HasKey(c => c.Id);
        modelBuilder.Entity<ToDo>().Property(b => b.Label).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<ToDo>().Property(b => b.IsDone).HasMaxLength(5);

        modelBuilder.Entity<ApplicationUser>().HasKey(u => u.Id);
        modelBuilder.Entity<ApplicationUser>().Property(b => b.Login).HasMaxLength(50).IsRequired();
        modelBuilder.Entity<ApplicationUser>().HasIndex(l => l.Login).IsUnique();
        modelBuilder.Entity<ApplicationUser>().Navigation(e => e.Roles).AutoInclude();

        modelBuilder.Entity<ApplicationUserApplicationRole>().HasKey(u => new { u.ApplicationUserId, u.ApplicationUserRoleId });
        modelBuilder.Entity<ApplicationUserApplicationRole>().Navigation(e => e.ApplicationUserRole).AutoInclude();

        modelBuilder.Entity<ApplicationUser>().HasMany(e => e.Roles).WithOne(e => e.ApplicationUser).HasForeignKey(e => e.ApplicationUserId);

        modelBuilder.Entity<ApplicationUserRole>().HasMany(e => e.Users).WithOne(e => e.ApplicationUserRole).HasForeignKey(e => e.ApplicationUserRoleId);

        modelBuilder.Entity<ToDo>()
            .HasOne(u => u.User)
            .WithMany(t => t.ToDos)
            .HasForeignKey(u => u.UserId);

        modelBuilder.Entity<ApplicationUserRole>().HasKey(u => u.Id);
        modelBuilder.Entity<ApplicationUserRole>().Property(b => b.Name).HasMaxLength(50).IsRequired();

        modelBuilder.Entity<RefreshToken>().HasKey(i => i.Id);
        modelBuilder.Entity<RefreshToken>().Property(i => i.Id).HasDefaultValueSql("NEWID()");

        base.OnModelCreating(modelBuilder);
    }
}