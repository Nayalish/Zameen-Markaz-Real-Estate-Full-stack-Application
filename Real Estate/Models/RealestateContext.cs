using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using static Real_Estate.Models.User;

namespace Real_Estate.Models;

public partial class RealestateContext : DbContext
{
    public RealestateContext()
    {
    }

    public RealestateContext(DbContextOptions<RealestateContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Agent> Agents { get; set; }
    public virtual DbSet<RentProperty> RentPropertys { get; set; }
    public virtual DbSet<SaleProperty> SalePropertys { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-ALAB59H\\SQLEXPRESS;Initial Catalog=Realestate;User ID=nayalish;Password=nayalish;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
