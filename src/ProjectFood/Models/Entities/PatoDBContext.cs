using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProjectFood.Models.Entities
{
    public partial class PatoDBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer(@"Server=tcp:patodb.database.windows.net,1433;Initial Catalog=PatoDB;Persist Security Info=False;User ID=PatoDBAdmin;Password=Sommar2016!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category", "Loula");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<FoodItem>(entity =>
            {
                entity.ToTable("FoodItem", "Loula");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.FoodType)
                    .WithMany(p => p.FoodItem)
                    .HasForeignKey(d => d.FoodTypeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_FoodItem_FoodType");
            });

            modelBuilder.Entity<FoodItemCategory>(entity =>
            {
                entity.ToTable("FoodItemCategory", "Loula");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.FoodItemCategory)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_FoodItemCategory_Category");

                entity.HasOne(d => d.FoodItem)
                    .WithMany(p => p.FoodItemCategory)
                    .HasForeignKey(d => d.FoodItemId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_FoodItemCategory_FoodItem");
            });

            modelBuilder.Entity<FoodType>(entity =>
            {
                entity.ToTable("FoodType", "Loula");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<KitchenStorage>(entity =>
            {
                entity.ToTable("KitchenStorage", "Loula");
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.ToTable("Recipe", "Loula");

                entity.Property(e => e.ImageUrl)
                    .HasColumnName("ImageURL")
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.Instructions)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<RecipeFoodItem>(entity =>
            {
                entity.ToTable("RecipeFoodItem", "Loula");

                entity.HasOne(d => d.FoodItem)
                    .WithMany(p => p.RecipeFoodItem)
                    .HasForeignKey(d => d.FoodItemId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_RecipeFoodItem_FoodItem");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User", "Loula");

                entity.Property(e => e.AspNetId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.KitchenStorage)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.KitchenStorageId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_User_KitchenStorage");
            });

            modelBuilder.Entity<UserFoodItem>(entity =>
            {
                entity.ToTable("UserFoodItem", "Loula");

                entity.Property(e => e.Expires).HasColumnType("date");

                entity.HasOne(d => d.FoodItem)
                    .WithMany(p => p.UserFoodItem)
                    .HasForeignKey(d => d.FoodItemId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_UserFoodItem_FoodItem");

                entity.HasOne(d => d.KitchenStorage)
                    .WithMany(p => p.UserFoodItem)
                    .HasForeignKey(d => d.KitchenStorageId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_UserFoodItem_KitchenStorage");
            });
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<FoodItem> FoodItem { get; set; }
        public virtual DbSet<FoodItemCategory> FoodItemCategory { get; set; }
        public virtual DbSet<FoodType> FoodType { get; set; }
        public virtual DbSet<KitchenStorage> KitchenStorage { get; set; }
        public virtual DbSet<Recipe> Recipe { get; set; }
        public virtual DbSet<RecipeFoodItem> RecipeFoodItem { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserFoodItem> UserFoodItem { get; set; }
    }
}