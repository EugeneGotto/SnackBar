using Microsoft.AspNet.Identity.EntityFramework;
using SnackBar.DAL.Interfaces;
using SnackBar.DAL.Models;
using SnackBar.DAL.Models.Identity;
using SnackBar.DAL.Repositories;
using System.Data.Entity;

namespace SnackBar.DAL.Context
{
    public class BarContext : IdentityDbContext<ApplicationUser>, IDataContext
    {
        public BarContext() : base("BarConnection")
        {
        }

        public static BarContext Create()
        {
            return new BarContext();
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Buyer> Buyers { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Tag> Tags { get; set; }

        //public DbSet<TagProduct> TagProducts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<Order>().Property(p => p.ProductId).IsRequired();
            modelBuilder.Entity<Order>().Property(p => p.Pin).IsRequired();
            modelBuilder.Entity<Order>().Property(p => p.UserId).IsRequired();
            modelBuilder.Entity<Order>().Property(p => p.Price).IsRequired();
            modelBuilder.Entity<Order>().HasRequired(p => p.Product);
            modelBuilder.Entity<Order>().HasRequired(p => p.User);
            modelBuilder.Entity<Order>().Property(p => p.Count).IsOptional();
            modelBuilder.Entity<Order>().Ignore(p => p.TotalPrice);

            modelBuilder.Entity<Payment>().ToTable("Payments");
            modelBuilder.Entity<Payment>().Property(p => p.UserId).IsRequired();
            modelBuilder.Entity<Payment>().Property(p => p.Money).IsRequired();
            modelBuilder.Entity<Payment>().HasRequired(p => p.User);

            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Product>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Product>().Property(p => p.Count).IsRequired();
            modelBuilder.Entity<Product>().Property(p => p.Price).IsRequired();
            modelBuilder.Entity<Product>().Property(p => p.Barcode).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Product>().HasMany(c => c.Tags)
                                            .WithMany(s => s.Products)
                                            .Map(t => t.MapLeftKey("ProductId")
                                            .MapRightKey("TagId")
                                            .ToTable("TagProduct"));

            modelBuilder.Entity<Buyer>().ToTable("Buyers");
            modelBuilder.Entity<Buyer>().Property(p => p.Email).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Buyer>().Property(p => p.Pin).IsRequired().HasMaxLength(4);
            modelBuilder.Entity<Buyer>().Property(p => p.Balance).IsRequired();
            modelBuilder.Entity<Buyer>().Property(p => p.Password).IsOptional().HasMaxLength(10).HasColumnType("nvarchar");

            modelBuilder.Entity<CartItem>().ToTable("CartItems");
            modelBuilder.Entity<CartItem>().Property(p => p.BuyerId).IsRequired();
            modelBuilder.Entity<CartItem>().Property(p => p.ProductId).IsRequired();
            modelBuilder.Entity<CartItem>().Property(p => p.Count).IsRequired();
            modelBuilder.Entity<CartItem>().Ignore(p => p.TotalPrice);
            modelBuilder.Entity<CartItem>().HasRequired(p => p.Buyer);
            modelBuilder.Entity<CartItem>().HasRequired(p => p.Product);

            modelBuilder.Entity<Tag>().ToTable("Tags");
            modelBuilder.Entity<Tag>().Property(p => p.TagName).IsRequired();
            modelBuilder.Entity<Tag>().Property(p => p.TagNameRu).IsOptional();

            //modelBuilder.Entity<TagProduct>().ToTable("TagProducts");
            //modelBuilder.Entity<TagProduct>().Property(p => p.TagId).IsRequired();
            //modelBuilder.Entity<TagProduct>().Property(p => p.ProductId).IsRequired();

            base.OnModelCreating(modelBuilder);
        }

        public void SyncObjectState<TModel>(TModel entity) where TModel : class, IObjectState
        {
            Entry(entity).State = StateHelper.ConvertState(entity.ObjectState);
        }

        private void SyncObjectsStatePreCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
            {
                dbEntityEntry.State = StateHelper.ConvertState(((IObjectState)dbEntityEntry.Entity).ObjectState);
            }
        }

        public void SyncObjectsStatePostCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
            {
                ((IObjectState)dbEntityEntry.Entity).ObjectState = StateHelper.ConvertState(dbEntityEntry.State);
            }
        }
    }
}