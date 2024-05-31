using FileGuardian.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileGuardian.Infrastructure.Persistence;

public class FileGuardianDbContext(DbContextOptions<FileGuardianDbContext> options) : DbContext(options)
{
    public DbSet<File> Files { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }

    // join tables
    public DbSet<GroupUser> GroupUsers { get; set; }
    public DbSet<FileUser> FileUsers { get; set; }
    public DbSet<FileGroup> FileGroups { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<File>(entity =>
        {
            entity.Property(f => f.Name).IsRequired();
            entity.Property(f => f.Risk).IsRequired();
            entity.HasIndex(f => f.Risk); // this will improve performance when sorting by risk
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(f => f.Name).IsRequired();
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.Property(f => f.Name).IsRequired();
        });

        modelBuilder.Entity<FileUser>().HasKey(fu => new { fu.FileId, fu.UserId });
        modelBuilder.Entity<FileGroup>().HasKey(fg => new { fg.FileId, fg.GroupId });
        modelBuilder.Entity<GroupUser>().HasKey(gu => new { gu.GroupId, gu.UserId });
    }
}