using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DentalLabManagement.DataTier.Models
{
    public partial class DentalLabManagementContext : DbContext
    {
        public DentalLabManagementContext()
        {
        }

        public DentalLabManagementContext(DbContextOptions<DentalLabManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Dental> Dentals { get; set; } = null!;
        public virtual DbSet<GroupStage> GroupStages { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;
        public virtual DbSet<OrderItemStage> OrderItemStages { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductStage> ProductStages { get; set; } = null!;
        public virtual DbSet<TeethPosition> TeethPositions { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<WarrantyCard> WarrantyCards { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=Khoa\\SQLEXPRESS;Initial Catalog=DentalLabManagement;\nPersist Security Info=True;User ID=sa;Password=12345");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Role)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.CategoryName).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(50);
            });

            modelBuilder.Entity<Dental>(entity =>
            {
                entity.ToTable("Dental");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Dentals)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dental_Account1");
            });

            modelBuilder.Entity<GroupStage>(entity =>
            {
                entity.ToTable("GroupStage");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.GroupStages)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupStage_Category");

                entity.HasOne(d => d.ProductStage)
                    .WithMany(p => p.GroupStages)
                    .HasForeignKey(d => d.ProductStageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupStage_ProductStage1");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.CompletedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DentistName).HasMaxLength(50);

                entity.Property(e => e.DentistNote).HasMaxLength(50);

                entity.Property(e => e.InvoiceId)
                    .HasMaxLength(6)
                    .IsFixedLength();

                entity.Property(e => e.Mode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PatientGender)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PatientName).HasMaxLength(50);

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Dental)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.DentalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Dental");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_Order_Account1");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("OrderItem");

                entity.Property(e => e.Note).HasMaxLength(50);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderItem_Order");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderItem_Product");

                entity.HasOne(d => d.TeethPosition)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.TeethPositionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderItems_TeethPosition");

                entity.HasOne(d => d.Warranty)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.WarrantyId)
                    .HasConstraintName("FK_OrderItems_WarrantyCard");
            });

            modelBuilder.Entity<OrderItemStage>(entity =>
            {
                entity.ToTable("OrderItemStage");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.Image).IsUnicode(false);

                entity.Property(e => e.Note).HasMaxLength(50);

                entity.Property(e => e.StageName).HasMaxLength(50);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.OrderItem)
                    .WithMany(p => p.OrderItemStages)
                    .HasForeignKey(d => d.OrderItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderItemStage_OrderItem");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.OrderItemStages)
                    .HasForeignKey(d => d.StaffId)
                    .HasConstraintName("FK_OrderItemStage_Account");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.Note).HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Payment_Order");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Category");
            });

            modelBuilder.Entity<ProductStage>(entity =>
            {
                entity.ToTable("ProductStage");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<TeethPosition>(entity =>
            {
                entity.ToTable("TeethPosition");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.PositionName)
                    .HasMaxLength(3)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");

                entity.Property(e => e.Note).HasMaxLength(50);

                entity.Property(e => e.PaymentId)
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.Status).HasMaxLength(50);
            });

            modelBuilder.Entity<WarrantyCard>(entity =>
            {
                entity.ToTable("WarrantyCard");

                entity.Property(e => e.CardCode).HasMaxLength(50);

                entity.Property(e => e.CardType).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.PatientName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
