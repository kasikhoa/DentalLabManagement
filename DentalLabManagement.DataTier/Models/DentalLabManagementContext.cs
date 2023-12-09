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
        public virtual DbSet<CardType> CardTypes { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<ExportMaterial> ExportMaterials { get; set; } = null!;
        public virtual DbSet<ExtraProductMapping> ExtraProductMappings { get; set; } = null!;
        public virtual DbSet<ImportMaterial> ImportMaterials { get; set; } = null!;
        public virtual DbSet<MaterialStock> MaterialStocks { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderHistory> OrderHistories { get; set; } = null!;
        public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;
        public virtual DbSet<OrderItemHistory> OrderItemHistories { get; set; } = null!;
        public virtual DbSet<OrderItemStage> OrderItemStages { get; set; } = null!;
        public virtual DbSet<Partner> Partners { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductStageMapping> ProductStageMappings { get; set; } = null!;
        public virtual DbSet<ProductionStage> ProductionStages { get; set; } = null!;
        public virtual DbSet<Promotion> Promotions { get; set; } = null!;
        public virtual DbSet<PromotionOrderMapping> PromotionOrderMappings { get; set; } = null!;
        public virtual DbSet<PromotionProductMapping> PromotionProductMappings { get; set; } = null!;
        public virtual DbSet<TeethPosition> TeethPositions { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<WarrantyCard> WarrantyCards { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(local);Database=DentalLabManagement;\nPersist Security Info=True;User ID=sa;Password=12345");
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

                entity.HasOne(d => d.CurrentStageNavigation)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.CurrentStage)
                    .HasConstraintName("FK_Account_ProductionStage");
            });

            modelBuilder.Entity<CardType>(entity =>
            {
                entity.ToTable("CardType");

                entity.Property(e => e.BrandUrl)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CodeName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CountryOrigin).HasMaxLength(20);

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Image).IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.CardTypes)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CardType_Category");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Image).IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(50);
            });

            modelBuilder.Entity<ExportMaterial>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ExportMaterial");

                entity.Property(e => e.ExportDate).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Note).HasMaxLength(50);

                entity.HasOne(d => d.MaterialStock)
                    .WithMany()
                    .HasForeignKey(d => d.MaterialStockId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExportMaterial_MaterialStock");
            });

            modelBuilder.Entity<ExtraProductMapping>(entity =>
            {
                entity.ToTable("ExtraProductMapping");

                entity.HasOne(d => d.ExtraProduct)
                    .WithMany(p => p.ExtraProductMappingExtraProducts)
                    .HasForeignKey(d => d.ExtraProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductExtraMapping_Product1");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ExtraProductMappingProducts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductExtraMapping_Product");
            });

            modelBuilder.Entity<ImportMaterial>(entity =>
            {
                entity.ToTable("ImportMaterial");

                entity.Property(e => e.BillCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ImportDate).HasColumnType("datetime");

                entity.Property(e => e.Note).HasMaxLength(50);

                entity.HasOne(d => d.MaterialStock)
                    .WithMany(p => p.ImportMaterials)
                    .HasForeignKey(d => d.MaterialStockId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ImportMaterial_MaterialStock");
            });

            modelBuilder.Entity<MaterialStock>(entity =>
            {
                entity.ToTable("MaterialStock");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.MaterialStocks)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Material_Category");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.CompletedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DentistName).HasMaxLength(50);

                entity.Property(e => e.DentistNote).HasMaxLength(200);

                entity.Property(e => e.InvoiceId)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Note).HasMaxLength(50);

                entity.Property(e => e.PatientGender)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PatientName).HasMaxLength(50);

                entity.Property(e => e.PatientPhoneNumber)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PartnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Dental");
            });

            modelBuilder.Entity<OrderHistory>(entity =>
            {
                entity.ToTable("OrderHistory");

                entity.Property(e => e.CompletedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Note).HasMaxLength(50);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.OrderHistories)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WarrantyHistory_Account1");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderHistories)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WarrantyHistory_Order");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("OrderItem");

                entity.Property(e => e.Mode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

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
                    .HasConstraintName("FK_OrderItem_Product1");

                entity.HasOne(d => d.TeethPosition)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.TeethPositionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderItem_TeethPosition");

                entity.HasOne(d => d.WarrantyCard)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.WarrantyCardId)
                    .HasConstraintName("FK_OrderItems_WarrantyCard");
            });

            modelBuilder.Entity<OrderItemHistory>(entity =>
            {
                entity.ToTable("OrderItemHistory");

                entity.Property(e => e.CompletedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Note).HasMaxLength(50);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.OrderItem)
                    .WithMany(p => p.OrderItemHistories)
                    .HasForeignKey(d => d.OrderItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderItemHistory_OrderItem");
            });

            modelBuilder.Entity<OrderItemStage>(entity =>
            {
                entity.ToTable("OrderItemStage");

                entity.Property(e => e.CompletedTime).HasColumnType("datetime");

                entity.Property(e => e.ExpectedTime).HasColumnType("datetime");

                entity.Property(e => e.Image).IsUnicode(false);

                entity.Property(e => e.Mode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Note).HasMaxLength(50);

                entity.Property(e => e.StartTime).HasColumnType("datetime");

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

                entity.HasOne(d => d.Stage)
                    .WithMany(p => p.OrderItemStages)
                    .HasForeignKey(d => d.StageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderItemStage_ProductionStage");
            });

            modelBuilder.Entity<Partner>(entity =>
            {
                entity.ToTable("Partner");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Partners)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_Dental_Account");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.Note).HasMaxLength(50);

                entity.Property(e => e.PaymentTime).HasColumnType("datetime");

                entity.Property(e => e.PaymentType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

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

                entity.Property(e => e.Image).IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Category");
            });

            modelBuilder.Entity<ProductStageMapping>(entity =>
            {
                entity.ToTable("ProductStageMapping");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductStageMappings)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductStageMapping_Product1");

                entity.HasOne(d => d.Stage)
                    .WithMany(p => p.ProductStageMappings)
                    .HasForeignKey(d => d.StageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductStageMapping_ProductionStage");
            });

            modelBuilder.Entity<ProductionStage>(entity =>
            {
                entity.ToTable("ProductionStage");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Promotion>(entity =>
            {
                entity.ToTable("Promotion");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PromotionOrderMapping>(entity =>
            {
                entity.ToTable("PromotionOrderMapping");

                entity.Property(e => e.EffectType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VoucherCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.PromotionOrderMappings)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_PromotionOrderMapping_Order");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.PromotionOrderMappings)
                    .HasForeignKey(d => d.PromotionId)
                    .HasConstraintName("FK_PromotionOrderMapping_Promotion");
            });

            modelBuilder.Entity<PromotionProductMapping>(entity =>
            {
                entity.ToTable("PromotionProductMapping");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.PromotionProductMappings)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_PromotionProductMapping_Product");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.PromotionProductMappings)
                    .HasForeignKey(d => d.PromotionId)
                    .HasConstraintName("FK_PromotionProductMapping_Promotion");
            });

            modelBuilder.Entity<TeethPosition>(entity =>
            {
                entity.ToTable("TeethPosition");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Image).IsUnicode(false);

                entity.Property(e => e.PositionName)
                    .HasMaxLength(3)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");

                entity.Property(e => e.Note).HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.PaymentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_Payment");
            });

            modelBuilder.Entity<WarrantyCard>(entity =>
            {
                entity.ToTable("WarrantyCard");

                entity.Property(e => e.CardCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExpDate).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.CardType)
                    .WithMany(p => p.WarrantyCards)
                    .HasForeignKey(d => d.CardTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WarrantyCard_CardType");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
