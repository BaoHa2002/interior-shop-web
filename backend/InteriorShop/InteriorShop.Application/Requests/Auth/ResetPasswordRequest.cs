using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorShop.Application.Requests.Auth
{
    public class ResetPasswordRequest
    {
        public string Token { get; set; } = default!;   // Token trong email
        public string Email { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
        public string ConfirmNewPassword { get; set; } = default!;
    }
}