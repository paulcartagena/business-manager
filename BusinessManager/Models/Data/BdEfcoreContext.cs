using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

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

    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<InventoryMovement> InventoryMovements { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Purchase> Purchases { get; set; }
    public virtual DbSet<Rol> Rols { get; set; }
    public virtual DbSet<Sale> Sales { get; set; }
    public virtual DbSet<Supplier> Suppliers { get; set; }
    public virtual DbSet<Uom> Uoms { get; set; }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<InventoryMovement>()
            .Property(e => e.MovementType)
            .HasConversion<string>()
            .HasMaxLength(20);
    }
}
