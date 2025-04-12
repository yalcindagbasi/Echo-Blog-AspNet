using BlogAspNet.Web.Models.Entities;
using BlogAspNet.Web.Models.Repositories.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogAspNet.Web.Models.Repositories;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Comment> Comments { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Blog>()
            .HasOne(b => b.User)
            .WithMany(u => u.Blogs)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.Content).HasMaxLength(10000);
            entity.Property(e => e.ImageUrl).HasMaxLength(400);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
        });

        modelBuilder.Entity<Blog>()
            .HasOne(b => b.Category)
            .WithMany(c => c.Blogs)
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Blog>()
            .Property(b => b.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        modelBuilder.Entity<Blog>()
            .Property(b => b.UpdatedAt)
            .HasDefaultValueSql("GETUTCDATE()");
        
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Blog)
            .WithMany(b => b.Comments)
            .HasForeignKey(c => c.BlogId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}