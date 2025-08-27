using InteriorShop.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorShop.Application.Interfaces
{
    public interface ITokenService
    {
        TokenResult CreateToken(JwtUserInfo user, IEnumerable<string> roles);
    }
}
