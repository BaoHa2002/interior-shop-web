using InteriorShop.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace InteriorShop.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>, IAppUser
    {
        public string FullName { get; set; } = default!;

        // Email sẽ dùng để login
        public override string Email { get; set; } = default!;

        // SĐT bắt buộc để công ty quản lý khách hàng
        public override string PhoneNumber { get; set; } = default!;

        // Trạng thái tài khoản
        public bool IsActive { get; set; } = true;

        // Ngày tạo tài khoản
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
