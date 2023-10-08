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
        public virtual DbSet<Dentist> Dentists { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<GroupStage> GroupStages { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;
        public virtual DbSet<OrderItemStage> OrderItemStages { get; set; } = null!;
        public virtual DbSet<Patient> Patients { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductStage> ProductStages { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Stage> Stages { get; set; } = null!;
        public virtual DbSet<TeethPosition> TeethPositions { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<WarrantyCard> WarrantyCards { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=Khoa\\SQLEXPRESS;Initial Catalog=DentalLabManagement;Persist Security Info=True;User ID=sa;Password=12345");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Role)
                    .HasMaxLength(50)
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

                entity.Property(e => e.DentalName).HasMaxLength(50);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Dentals)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dental_Account1");
            });

            modelBuilder.Entity<Dentist>(entity =>
            {
                entity.ToTable("Dentist");

                entity.Property(e => e.DentistName).HasMaxLength(50);

                entity.HasOne(d => d.Dental)
                    .WithMany(p => p.Dentists)
                    .HasForeignKey(d => d.DentalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dentist_Dental");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<GroupStage>(entity =>
            {
                entity.ToTable("GroupStage");

                entity.Property(e => e.StageId)
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.GroupStages)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_GroupStage_Product1");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.CompletedDate).HasColumnType("date");

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.InvoiceId)
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.Mode)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.NameDentist).HasMaxLength(50);

                entity.Property(e => e.PatientName).HasMaxLength(50);

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.HasOne(d => d.Dental)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.DentalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Dental");

                entity.HasOne(d => d.PatientPhoneNumberNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PatientPhoneNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Patient");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("OrderItem");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderTeeth_Order1");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderItem_Product");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeethProduct_Account");

                entity.HasOne(d => d.TeethPosition)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.TeethPositionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderItems_TeethPosition");

                entity.HasOne(d => d.Warranty)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.WarrantyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderItems_WarrantyCard");
            });

            modelBuilder.Entity<OrderItemStage>(entity =>
            {
                entity.ToTable("OrderItemStage");

                entity.Property(e => e.EndDate).HasMaxLength(50);

                entity.Property(e => e.Image).HasColumnType("image");

                entity.Property(e => e.Note).HasMaxLength(50);

                entity.Property(e => e.StageId)
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.StartDate).HasMaxLength(50);

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.TeethProduct)
                    .WithMany(p => p.OrderItemStages)
                    .HasForeignKey(d => d.TeethProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderItemStage_TeethProduct");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("Patient");

                entity.Property(e => e.Name).HasMaxLength(50);
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

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.GroupStaget)
                    .WithMany(p => p.ProductStages)
                    .HasForeignKey(d => d.GroupStagetId)
                    .HasConstraintName("FK_ProductStage_GroupStage");

                entity.HasOne(d => d.OrderItemStage)
                    .WithMany(p => p.ProductStages)
                    .HasForeignKey(d => d.OrderItemStageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductStage_OrderItemStage");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            modelBuilder.Entity<Stage>(entity =>
            {
                entity.ToTable("Stage");

                entity.Property(e => e.StageName).HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(50);
            });

            modelBuilder.Entity<TeethPosition>(entity =>
            {
                entity.ToTable("TeethPosition");

                entity.Property(e => e.Position)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.ProductId)
                    .HasMaxLength(10)
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

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("endDate");

                entity.Property(e => e.OrderItemsId)
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("startDate");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.WarrantyCards)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WarrantyCard_Patient");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
