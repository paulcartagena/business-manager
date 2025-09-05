using BusinessManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace BusinessManager.Models;

public partial class BdEfcoreContext : DbContext
{
    public BdEfcoreContext()
    {
    }

    public BdEfcoreContext(DbContextOptions<BdEfcoreContext> options)
        : base(options)
    {
    }

    // DbSets 
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Rol> Rols { get; set; }
    public virtual DbSet<Uom> Uoms { get; set; }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración de Category
        ConfigureCategory(modelBuilder);

        // Configuración de Product
        ConfigureProduct(modelBuilder);

        // Configuración de Rol
        ConfigureRol(modelBuilder);

        // Configuración de Uom
        ConfigureUom(modelBuilder);

        // Configuración de User
        ConfigureUser(modelBuilder);

        OnModelCreatingPartial(modelBuilder);
    }

    #region Entity Configurations

    private static void ConfigureCategory(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A2BAAC422D5");
            entity.ToTable("Category");

            // Propiedades
            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnName("name");
        });
    }

    private static void ConfigureProduct(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__B40CC6ED940D9C21");
            entity.ToTable("Product");

            // Propiedades
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnName("name");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.UomId).HasColumnName("uomId");
            entity.Property(e => e.SalePrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("SalePrice");
            entity.Property(e => e.PurchasePrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("PurchasePrice");
            entity.Property(e => e.Stock)
                .HasDefaultValue(0)
                .HasColumnName("stock");

            // Relaciones
            entity.HasOne(d => d.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Product_Category");

            entity.HasOne(d => d.Uom)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.UomId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Product_Uom");
        });
    }

    private static void ConfigureRol(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.RolId).HasName("PK__Rol__F92302D11BE26585");
            entity.ToTable("Rol");

            // Propiedades
            entity.Property(e => e.RolId).HasColumnName("rolId");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnName("name");
        });
    }

    private static void ConfigureUom(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Uom>(entity =>
        {
            entity.HasKey(e => e.UomId).HasName("PK__Uom__03D77E7656DCB8D7");
            entity.ToTable("Uom");

            // Propiedades
            entity.Property(e => e.UomId).HasColumnName("uomId");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnName("name");
            entity.Property(e => e.IsWeightUnit)
                .HasDefaultValue(false)
                .HasColumnName("isWeightUnit");
        });
    }

    private static void ConfigureUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Person__1788CC4CA353C9FC");
            entity.ToTable("User");

            // Propiedades
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnName("name");
            entity.Property(e => e.Mail)
                .HasMaxLength(100)
                .HasColumnName("mail");
            entity.Property(e => e.RolId).HasColumnName("rolId");

            // Relaciones
            entity.HasOne(d => d.Rol)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_User_Rol");
        });
    }

    #endregion

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}