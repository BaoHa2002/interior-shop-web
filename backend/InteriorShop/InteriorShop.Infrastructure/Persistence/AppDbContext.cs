using InteriorShop.Domain.Entities;
using InteriorShop.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace InteriorShop.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductImage> ProductImages => Set<ProductImage>();
        public DbSet<OptionGroup> OptionGroups => Set<OptionGroup>();
        public DbSet<OptionValue> OptionValues => Set<OptionValue>();
        public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();

        // Banners
        public DbSet<BannerSlide> BannerSlides => Set<BannerSlide>();
        public DbSet<PromoTile> PromoTiles => Set<PromoTile>();
        public DbSet<CampaignBanner> CampaignBanners => Set<CampaignBanner>();

        // Content
        public DbSet<BlogPost> BlogPosts => Set<BlogPost>();

        // Orders
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        // Settings & Auth
        public DbSet<SiteSetting> SiteSettings => Set<SiteSetting>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            // ===== CATEGORY =====
            b.Entity<Category>(e =>
            {
                e.HasIndex(x => x.Slug).IsUnique();
                e.HasOne(x => x.Parent)
                 .WithMany(x => x.Children)
                 .HasForeignKey(x => x.ParentId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== PRODUCT =====
            b.Entity<Product>(e =>
            {
                e.HasIndex(x => x.Slug).IsUnique();
                e.HasIndex(x => x.Sku).IsUnique();
                e.Property(x => x.Price).HasPrecision(18, 2);
                e.Property(x => x.SalePrice).HasPrecision(18, 2);

                // many-to-many
                e.HasMany(x => x.Categories).WithMany(c => c.Products);
            });

            // ===== PRODUCT IMAGE =====
            b.Entity<ProductImage>(e =>
            {
                e.HasOne(x => x.Product)
                 .WithMany(p => p.Images)
                 .HasForeignKey(x => x.ProductId);
            });

            // ===== OPTION GROUP =====
            b.Entity<OptionGroup>(e =>
            {
                e.HasOne(x => x.Product)
                 .WithMany(p => p.OptionsGroups)
                 .HasForeignKey(x => x.ProductId)
                 .OnDelete(DeleteBehavior.Cascade); // OK vì ProductId là nullable; chỉ xoá khi gắn vào product
            });

            // ===== OPTION VALUE =====
            b.Entity<OptionValue>(e =>
            {
                e.HasOne(x => x.OptionGroup)
                 .WithMany(g => g.Values)
                 .HasForeignKey(x => x.OptionGroupId);
            });

            // ===== PRODUCT VARIANT =====
            b.Entity<ProductVariant>(e =>
            {
                e.HasOne(x => x.Product)
                 .WithMany(p => p.Variants)
                 .HasForeignKey(x => x.ProductId);

                e.HasIndex(x => x.Sku).IsUnique();
                e.Property(x => x.Price).HasPrecision(18, 2);
                e.Property(x => x.SalePrice).HasPrecision(18, 2);
                e.Property(x => x.OptionsJson).HasMaxLength(2000);
            });

            // ===== ORDER =====
            b.Entity<Order>(e =>
            {
                e.HasIndex(x => x.Code).IsUnique();
                e.Property(x => x.Subtotal).HasPrecision(18, 2);
                e.Property(x => x.ShippingFee).HasPrecision(18, 2);
                e.Property(x => x.Total).HasPrecision(18, 2);

                // Nếu trong Order có ICollection<OrderItem> Items:
                e.HasMany(x => x.Items)
                 .WithOne(i => i.Order)
                 .HasForeignKey(i => i.OrderId)
                 .OnDelete(DeleteBehavior.Cascade); // Nếu xóa Order thì xóa luôn OrderItems
            });

            // ===== ORDER ITEM =====
            b.Entity<OrderItem>(e =>
            {
                e.Property(x => x.UnitPrice).HasPrecision(18, 2);
                e.Property(x => x.LineTotal).HasPrecision(18, 2);
            });

            // ===== BLOG POST =====
            b.Entity<BlogPost>(e =>
            {
                e.HasIndex(x => x.Slug).IsUnique();
            });

            // ===== REFRESH TOKEN =====
            b.Entity<RefreshToken>(e =>
            {
                e.HasIndex(x => x.Token).IsUnique();
            });

            // ===== PROMOS / BANNERS =====
            b.Entity<PromoTile>();
            b.Entity<BannerSlide>();
            b.Entity<CampaignBanner>();

            // ===== SITE SETTING (seed default) =====
            b.Entity<SiteSetting>(e =>
            {
                e.Property(x => x.DefaultShippingFee).HasPrecision(18, 2);
            });
        }
    }
}
