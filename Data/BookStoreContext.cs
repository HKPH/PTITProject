using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data;

public class BookStoreContext : DbContext
{
    public BookStoreContext(DbContextOptions<BookStoreContext> options)
        : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Publisher> Publishers { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<ShippingAddress> ShippingAddresses { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Account
        modelBuilder.Entity<Account>(entity =>
        {
            entity.Property(e => e.Username).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");

            entity.HasOne(e => e.User)
                  .WithMany(u => u.Accounts)
                  .HasForeignKey(e => e.UserId);
        });

        // Book
        modelBuilder.Entity<Book>(entity =>
        {
            entity.Property(e => e.Title);
            entity.Property(e => e.Author);
            entity.Property(e => e.Description);
            entity.Property(e => e.Image);
            entity.Property(e => e.Price).HasColumnType("decimal(18,3)");
            entity.Property(e => e.PublicationDate).HasColumnType("datetime");

            entity.HasOne(e => e.Publisher)
                  .WithMany(p => p.Books)
                  .HasForeignKey(e => e.PublisherId);

            entity.HasMany(e => e.Categories)
                  .WithMany(c => c.Books)
                  .UsingEntity(j => j.ToTable("JoinBookCategory"));
        });

        // Cart
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasOne(c => c.User)
                  .WithMany(u => u.Carts)
                  .HasForeignKey(c => c.UserId);
        });

        // CartItem
        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasOne(i => i.Cart)
                  .WithMany(c => c.CartItems)
                  .HasForeignKey(i => i.CartId)
                  .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(i => i.Book)
                  .WithMany(b => b.CartItems)
                  .HasForeignKey(i => i.BookId)
                  .OnDelete(DeleteBehavior.ClientSetNull);
        });

        // Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(c => c.Name).HasMaxLength(255);
        });

        // Order
        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(o => o.OrderDate).HasColumnType("datetime");
            entity.Property(o => o.TotalPrice).HasColumnType("decimal(18,3)");

            entity.HasOne(o => o.User)
                  .WithMany(u => u.Orders)
                  .HasForeignKey(o => o.UserId);

            entity.HasOne(o => o.Payment)
                  .WithMany(p => p.Orders)
                  .HasForeignKey(o => o.PaymentId);

            entity.HasOne(o => o.Shipment)
                  .WithMany(s => s.Orders)
                  .HasForeignKey(o => o.ShipmentId);
        });

        // OrderItem
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.Property(i => i.Price).HasColumnType("decimal(18,3)");
        });

        // Payment
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.Property(p => p.Amount).HasColumnType("decimal(18,3)");
            entity.Property(p => p.PaymentDate).HasColumnType("datetime");
            entity.Property(p => p.PaymentMethod).HasMaxLength(50);
        });

        // Publisher
        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.Property(p => p.Name);
        });

        // Rating
        modelBuilder.Entity<Rating>(entity =>
        {
            entity.Property(r => r.Comment).HasMaxLength(255);
            entity.Property(r => r.Photo).HasMaxLength(255);
            entity.Property(r => r.CreateDate).HasColumnType("datetime");

            entity.HasOne(r => r.Book)
                  .WithMany(b => b.Ratings)
                  .HasForeignKey(r => r.BookId);

            entity.HasOne(r => r.User)
                  .WithMany(u => u.Ratings)
                  .HasForeignKey(r => r.UserId);
        });

        // Shipment
        modelBuilder.Entity<Shipment>(entity =>
        {
            entity.Property(s => s.DateReceived).HasColumnType("datetime");
            entity.Property(s => s.Fee).HasColumnType("decimal(18,3)");

            entity.HasOne(s => s.ShippingAddress)
                  .WithMany(sa => sa.Shipments)
                  .HasForeignKey(s => s.ShippingAddressId);
        });

        // ShippingAddress
        modelBuilder.Entity<ShippingAddress>(entity =>
        {
            entity.Property(sa => sa.Address).HasMaxLength(255);
            entity.Property(sa => sa.CustomerNumber).HasMaxLength(50);
            entity.Property(sa => sa.Note).HasMaxLength(255);

            entity.HasOne(sa => sa.User)
                  .WithMany(u => u.ShippingAddresses)
                  .HasForeignKey(sa => sa.UserId);
        });

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.Name).HasMaxLength(255);
            entity.Property(u => u.Email).HasMaxLength(255);
            entity.Property(u => u.Phone).HasMaxLength(50);
            entity.Property(u => u.Address).HasMaxLength(255);
            entity.Property(u => u.Dob).HasColumnType("datetime");
        });
    }
}
