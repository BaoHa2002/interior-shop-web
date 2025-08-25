

using Microsoft.AspNetCore.Identity;

namespace InteriorShop.Infrastructure.Identity
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string? Description { get; set; }

        public ApplicationRole() : base()
        {

        }

        public ApplicationRole(string roleName, string? description = null) : base()
        {
            Description = description;
        }
    }
}
