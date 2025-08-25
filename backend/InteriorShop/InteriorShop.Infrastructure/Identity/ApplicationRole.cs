﻿using Microsoft.AspNetCore.Identity;

namespace InteriorShop.Infrastructure.Identity
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string? Description { get; set; }

        //Theo dõi ngày tạo role
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ApplicationRole() : base()
        {

        }

        public ApplicationRole(string roleName, string? description = null)
            : base(roleName)
        {
            Description = description;
        }
    }
}
