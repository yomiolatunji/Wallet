using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SBSC.Wallet.BusinessCore.DbModels;

public partial class WalletContext 
{
    public WalletContext()
    {
    }

    public WalletContext(DbContextOptions<WalletContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Audit> Audits { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Admins__3214EC07F12268B5");

            entity.HasIndex(e => e.Email, "UQ__Admins__A9D10534B675FE18").IsUnique();

            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DateDeleted).HasColumnType("datetime");
            entity.Property(e => e.DateUpdated).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.Password).HasMaxLength(256);
            entity.Property(e => e.Role).HasMaxLength(20);
        });

        modelBuilder.Entity<Audit>(entity =>
        {
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PrimaryKey).HasMaxLength(100);
            entity.Property(e => e.TableName).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.UserId).HasMaxLength(100);
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__currenci__3214EC07E7B83AAA");

            entity.Property(e => e.CurrencyLogoUrl).HasMaxLength(256);
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DateDeleted).HasColumnType("datetime");
            entity.Property(e => e.DateUpdated).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Symbol).HasMaxLength(10);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC0763791475");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.Narration).HasMaxLength(256);
            entity.Property(e => e.TransactionCode).HasMaxLength(10);
            entity.Property(e => e.TransactionDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.User).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__UserI__3F466844");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC0714D7E932");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053461202D95").IsUnique();

            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DateDeleted).HasColumnType("datetime");
            entity.Property(e => e.DateUpdated).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(256);
            entity.Property(e => e.ProfilePictureUrl).HasMaxLength(256);
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.Property(e => e.AcyBalance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AvailableBalance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreditRestricted)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .HasDefaultValueSql("(N'NGN')");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DateDeleted).HasColumnType("datetime");
            entity.Property(e => e.DateUpdated).HasColumnType("datetime");
            entity.Property(e => e.DebitRestricted)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.InterestReceiveable).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.LcyBalance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LienAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.WalletNumber).HasMaxLength(10);

            entity.HasOne(d => d.User).WithMany(p => p.Wallets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Wallets__UserId__44FF419A");
        });
        modelBuilder.HasSequence("SeqWalletNumber")
            .StartsAt(10000L)
            .HasMax(9999999999L);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
