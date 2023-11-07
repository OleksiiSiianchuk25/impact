using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EfCore;

public partial class ImpactDbContext : DbContext
{
    public ImpactDbContext()
    {
    }

    public ImpactDbContext(DbContextOptions<ImpactDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<RequestCategory> RequestCategories { get; set; }

    public virtual DbSet<RequestRole> RequestRoles { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=ep-empty-recipe-96792924.eu-central-1.aws.neon.tech;Database=impact-db;Username=sijanchuk;Password=nN8hVXe1pILY");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("requests_pkey");

            entity.ToTable("requests");

            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.ContactEmail)
                .HasMaxLength(255)
                .HasColumnName("contact_email");
            entity.Property(e => e.ContactPhone)
                .HasMaxLength(20)
                .HasColumnName("contact_phone");
            entity.Property(e => e.CreatorUserRef).HasColumnName("creator_user_ref");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .HasColumnName("location");
            entity.Property(e => e.RequestName)
                .HasMaxLength(255)
                .HasColumnName("request_name");
            entity.Property(e => e.RoleRef).HasColumnName("role_ref");

            entity.HasOne(d => d.CreatorUserRefNavigation).WithMany(p => p.Requests)
                .HasForeignKey(d => d.CreatorUserRef)
                .HasConstraintName("fk_creator_user");

            entity.HasOne(d => d.RoleRefNavigation).WithMany(p => p.Requests)
                .HasForeignKey(d => d.RoleRef)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_role");

            entity.HasMany(d => d.Categories).WithMany(p => p.Requests)
                .UsingEntity<Dictionary<string, object>>(
                    "RequestCategoriesMapping",
                    r => r.HasOne<RequestCategory>().WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_request_category"),
                    l => l.HasOne<Request>().WithMany()
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_request"),
                    j =>
                    {
                        j.HasKey("RequestId", "CategoryId").HasName("request_categories_mapping_pkey");
                        j.ToTable("request_categories_mapping");
                        j.IndexerProperty<int>("RequestId").HasColumnName("request_id");
                        j.IndexerProperty<int>("CategoryId").HasColumnName("category_id");
                    });
        });

        modelBuilder.Entity<RequestCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("request_categories_pkey");

            entity.ToTable("request_categories");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(255)
                .HasColumnName("category_name");
        });

        modelBuilder.Entity<RequestRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("request_roles_pkey");

            entity.ToTable("request_roles");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.HasIndex(e => e.PhoneNumber, "users_phone_number_key").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(255)
                .HasColumnName("middle_name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phone_number");
            entity.Property(e => e.RoleRef).HasColumnName("role_ref");

            entity.HasOne(d => d.RoleRefNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleRef)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
