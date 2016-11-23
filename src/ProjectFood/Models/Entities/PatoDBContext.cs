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
            });

            modelBuilder.Entity<FoodItemCategory>(entity =>
            {
                entity.ToTable("FoodItemCategory", "Loula");
            });

            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.ToTable("Ingredient", "Loula");

                entity.Property(e => e.Ingredient1).HasColumnName("Ingredient");

                entity.HasOne(d => d.Ingredient1Navigation)
                    .WithMany(p => p.Ingredient)
                    .HasForeignKey(d => d.Ingredient1)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Ingredient_FoodItem");
            });

            modelBuilder.Entity<KitchenStorage>(entity =>
            {
                entity.ToTable("KitchenStorage", "Loula");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.UserIngredientId).HasColumnName("UserIngredientID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.KitchenStorage)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_KitchenStorage_User");

                entity.HasOne(d => d.UserIngredient)
                    .WithMany(p => p.KitchenStorage)
                    .HasForeignKey(d => d.UserIngredientId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_KitchenStorage_UserIngredient");
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.ToTable("Recipe", "Loula");

                entity.Property(e => e.ImageUrl)
                    .HasColumnName("ImageURL")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Instructions)
                    .IsRequired()
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<RecipeCategory>(entity =>
            {
                entity.ToTable("RecipeCategory", "Loula");

                entity.Property(e => e.CategoryId).HasColumnName("Category_ID");

                entity.Property(e => e.RecipeId).HasColumnName("Recipe_ID");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.RecipeCategory)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_RecipeCategory_Category");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipeCategory)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_RecipeCategory_Recipe");
            });

            modelBuilder.Entity<RecipeIngredient>(entity =>
            {
                entity.ToTable("RecipeIngredient", "Loula");

                entity.Property(e => e.IngredientId).HasColumnName("Ingredient_ID");

                entity.Property(e => e.RecipeId).HasColumnName("Recipe_ID");

                entity.HasOne(d => d.Ingredient)
                    .WithMany(p => p.RecipeIngredient)
                    .HasForeignKey(d => d.IngredientId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_RecipeIngredient_Ingredient");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipeIngredient)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_RecipeIngredient_Recipe");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User", "Loula");

                entity.Property(e => e.AspNetId)
                    .IsRequired()
                    .HasMaxLength(450);
            });

            modelBuilder.Entity<UserIngredient>(entity =>
            {
                entity.ToTable("UserIngredient", "Loula");

                entity.Property(e => e.Expires).HasColumnType("date");

                entity.HasOne(d => d.IngredientNavigation)
                    .WithMany(p => p.UserIngredient)
                    .HasForeignKey(d => d.Ingredient)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_UserIngredient_Ingredient");
            });
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<FoodItem> FoodItem { get; set; }
        public virtual DbSet<FoodItemCategory> FoodItemCategory { get; set; }
        public virtual DbSet<Ingredient> Ingredient { get; set; }
        public virtual DbSet<KitchenStorage> KitchenStorage { get; set; }
        public virtual DbSet<Recipe> Recipe { get; set; }
        public virtual DbSet<RecipeCategory> RecipeCategory { get; set; }
        public virtual DbSet<RecipeIngredient> RecipeIngredient { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserIngredient> UserIngredient { get; set; }
    }
}