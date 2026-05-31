using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Trenia2.Models;

public partial class Trenia2Context : DbContext
{
    public Trenia2Context()
    {
    }

    public Trenia2Context(DbContextOptions<Trenia2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<EdIzm> EdIzms { get; set; }

    public virtual DbSet<Postavshik> Postavshiks { get; set; }

    public virtual DbSet<Proizvoditel> Proizvoditels { get; set; }

    public virtual DbSet<PunckVidachi> PunckVidachis { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SostavZakaz> SostavZakazs { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Tovar> Tovars { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Zakaz> Zakazs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=Trenia2;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCategory);

            entity.ToTable("Category");

            entity.Property(e => e.IdCategory).ValueGeneratedNever();
            entity.Property(e => e.Category1)
                .HasMaxLength(250)
                .HasColumnName("Category");
        });

        modelBuilder.Entity<EdIzm>(entity =>
        {
            entity.HasKey(e => e.IdEdIzm);

            entity.ToTable("EdIzm");

            entity.Property(e => e.IdEdIzm).ValueGeneratedNever();
            entity.Property(e => e.EdIzm1)
                .HasMaxLength(250)
                .HasColumnName("EdIzm");
        });

        modelBuilder.Entity<Postavshik>(entity =>
        {
            entity.HasKey(e => e.IdPost);

            entity.ToTable("Postavshik");

            entity.Property(e => e.IdPost).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(250);
        });

        modelBuilder.Entity<Proizvoditel>(entity =>
        {
            entity.HasKey(e => e.IdProizv);

            entity.ToTable("Proizvoditel");

            entity.Property(e => e.IdProizv).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(250);
        });

        modelBuilder.Entity<PunckVidachi>(entity =>
        {
            entity.HasKey(e => e.IdPuncta);

            entity.ToTable("PunckVidachi");

            entity.Property(e => e.IdPuncta).ValueGeneratedNever();
            entity.Property(e => e.Adres).HasMaxLength(250);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole);

            entity.ToTable("Role");

            entity.Property(e => e.IdRole).ValueGeneratedNever();
            entity.Property(e => e.Role1)
                .HasMaxLength(250)
                .HasColumnName("Role");
        });

        modelBuilder.Entity<SostavZakaz>(entity =>
        {
            entity.ToTable("SostavZakaz");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.IdTovarNavigation).WithMany(p => p.SostavZakazs)
                .HasForeignKey(d => d.IdTovar)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SostavZakaz_Tovar");

            entity.HasOne(d => d.IdZakazNavigation).WithMany(p => p.SostavZakazs)
                .HasForeignKey(d => d.IdZakaz)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SostavZakaz_Zakaz");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.IdStatus);

            entity.ToTable("Status");

            entity.Property(e => e.IdStatus).ValueGeneratedNever();
            entity.Property(e => e.Status1)
                .HasMaxLength(250)
                .HasColumnName("Status");
        });

        modelBuilder.Entity<Tovar>(entity =>
        {
            entity.HasKey(e => e.IdTovar);

            entity.ToTable("Tovar");

            entity.Property(e => e.IdTovar).ValueGeneratedNever();
            entity.Property(e => e.Articul).HasMaxLength(250);
            entity.Property(e => e.NameTovar).HasMaxLength(250);
            entity.Property(e => e.Photo).HasMaxLength(250);
            entity.Property(e => e.Price).HasColumnType("money");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Tovars)
                .HasForeignKey(d => d.IdCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tovar_Category");

            entity.HasOne(d => d.IdEdIzmNavigation).WithMany(p => p.Tovars)
                .HasForeignKey(d => d.IdEdIzm)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tovar_EdIzm");

            entity.HasOne(d => d.IdPostNavigation).WithMany(p => p.Tovars)
                .HasForeignKey(d => d.IdPost)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tovar_Postavshik");

            entity.HasOne(d => d.IdProizNavigation).WithMany(p => p.Tovars)
                .HasForeignKey(d => d.IdProiz)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tovar_Proizvoditel");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser);

            entity.ToTable("User");

            entity.Property(e => e.IdUser).ValueGeneratedNever();
            entity.Property(e => e.Fio).HasMaxLength(250);
            entity.Property(e => e.Login).HasMaxLength(250);
            entity.Property(e => e.Password).HasMaxLength(250);

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Role");
        });

        modelBuilder.Entity<Zakaz>(entity =>
        {
            entity.HasKey(e => e.IdZakaz);

            entity.ToTable("Zakaz");

            entity.Property(e => e.IdZakaz).ValueGeneratedNever();

            entity.HasOne(d => d.IdAdresNavigation).WithMany(p => p.Zakazs)
                .HasForeignKey(d => d.IdAdres)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Zakaz_PunckVidachi");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Zakazs)
                .HasForeignKey(d => d.IdStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Zakaz_Status");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Zakazs)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Zakaz_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
