using InteriorShop.Application.Common;
using InteriorShop.Application.DTOs.Auth;
using InteriorShop.Application.Interfaces;
using InteriorShop.Application.Requests.Auth;
using InteriorShop.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InteriorShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration config,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _emailService = emailService;
        }

        // --- REGISTER ---
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<TokenResult>>> Register([FromBody] RegisterRequest request)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return ApiResponse<TokenResult>.Fail(string.Join(", ", result.Errors.Select(e => e.Description)));

            return ApiResponse<TokenResult>.Ok(GenerateToken(user));
        }

        // --- LOGIN ---
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<TokenResult>>> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return ApiResponse<TokenResult>.Fail("Email không tồn tại");

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
                return ApiResponse<TokenResult>.Fail("Sai mật khẩu");

            return ApiResponse<TokenResult>.Ok(GenerateToken(user));
        }

        // --- CHANGE PASSWORD ---
        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult<ApiResponse<string>>> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return ApiResponse<string>.Fail("Không tìm thấy user");

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded)
                return ApiResponse<string>.Fail(string.Join(", ", result.Errors.Select(e => e.Description)));

            return ApiResponse<string>.Ok("Đổi mật khẩu thành công");
        }

        // --- FORGOT PASSWORD ---
        [HttpPost("forgot-password")]
        public async Task<ActionResult<ApiResponse<string>>> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return ApiResponse<string>.Ok("Nếu email tồn tại, link reset sẽ được gửi");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"{_config["App:FrontendUrl"]}/reset-password?email={user.Email}&token={Uri.EscapeDataString(token)}";

            await _emailService.SendAsync(user.Email!, "Reset Password",
                $"Click vào link sau để đặt lại mật khẩu: <a href='{resetLink}'>Reset Password</a>");

            return ApiResponse<string>.Ok("Nếu email tồn tại, link reset sẽ được gửi");
        }

        // --- RESET PASSWORD ---
        [HttpPost("reset-password")]
        public async Task<ActionResult<ApiResponse<string>>> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (request.NewPassword != request.ConfirmNewPassword)
                return ApiResponse<string>.Fail("Mật khẩu xác nhận không khớp");

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return ApiResponse<string>.Fail("Email không tồn tại");

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (!result.Succeeded)
                return ApiResponse<string>.Fail(string.Join(", ", result.Errors.Select(e => e.Description)));

            return ApiResponse<string>.Ok("Đặt lại mật khẩu thành công");
        }

        // --- Generate JWT ---
        private TokenResult GenerateToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim("fullName", user.FullName ?? "")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddHours(2);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return new TokenResult
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = expires
            };
        }
    }
}
