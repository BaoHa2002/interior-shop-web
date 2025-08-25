using InteriorShop.Domain.Entities;
using InteriorShop.Infrastructure.Identity;
using InteriorShop.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InteriorShop.Infrastructure.Seeders
{
    public static class DbSeeder
    {
        public static async Task SeederAsync(
            AppDbContext db,
            UserManager<ApplicationUser> userMgr,
            RoleManager<ApplicationRole> roleMgr)
        {
            // Ensure database is created & migrated
            await db.Database.MigrateAsync();

            // ===== ROLES =====
            if (!await roleMgr.Roles.AnyAsync())
            {
                await roleMgr.CreateAsync(new ApplicationRole { Name = "Admin" });
                await roleMgr.CreateAsync(new ApplicationRole { Name = "Customer" });
            }

            // ===== USER TẠM =====
            if (!await userMgr.Users.AnyAsync())
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin@phatdecors.local",
                    Email = "admin@phatdecors.local",
                    EmailConfirmed = true,
                    FullName = "Administrator",
                    PhoneNumber = "0123456789"
                };

                // tạo user với mật khẩu tạm
                var result = await userMgr.CreateAsync(admin, "Admin@123");
                if (result.Succeeded)
                {
                    await userMgr.AddToRoleAsync(admin, "Admin");
                }
            }

            // Seed SiteSetting mặc định
            if (!await db.SiteSettings.AnyAsync())
            {
                db.SiteSettings.Add(new SiteSetting
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    SiteName = "PhatDecors",
                    Hotline = "0364 988 789",
                    ShowroomAddress = "Tổ 2 , Khu phố 3 , Phường Trảng Dài, Biên Hòa, Đồng Nai",
                    DefaultShippingFee = 0,
                    LogoUrl = "/images/logo.png",
                    FaviconUrl = "/favicon.ico",
                    ContactEmail = ""
                    // các field Bank, SMTP để trống cho admin nhập sau
                });
                await db.SaveChangesAsync();
            }
        }
    }
}
