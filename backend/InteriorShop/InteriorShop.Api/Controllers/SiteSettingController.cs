using AutoMapper;
using InteriorShop.Application.Common;
using InteriorShop.Application.DTOs.Settings;
using InteriorShop.Application.Requests.Settings;
using InteriorShop.Domain.Entities;
using InteriorShop.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InteriorShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SiteSettingController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly ILogger<SiteSettingController> _logger;

        public SiteSettingController(AppDbContext db, IMapper mapper, ILogger<SiteSettingController> logger)
        {
            _db = db;
            _mapper = mapper;
            _logger = logger;
        }

        // ===== GET CURRENT SITE SETTING =====
        [HttpGet]
        public async Task<ActionResult<ApiResponse<SiteSettingDto>>> Get()
        {
            var setting = await _db.SiteSettings.FirstOrDefaultAsync();
            if (setting == null)
            {
                // Nếu chưa có thì tạo mặc định
                setting = new SiteSetting
                {
                    SiteName = "PhatDecors",
                    Hotline = "0123456789",
                    ShowroomAddress = "HCM City",
                    DefaultShippingFee = 0
                };
                _db.SiteSettings.Add(setting);
                await _db.SaveChangesAsync();
            }

            return ApiResponse<SiteSettingDto>.Ok(_mapper.Map<SiteSettingDto>(setting));
        }

        // ===== UPDATE SITE SETTING =====
        [HttpPut]
        public async Task<ActionResult<ApiResponse<SiteSettingDto>>> Update([FromBody] UpdateSiteSettingRequest req)
        {
            var setting = await _db.SiteSettings.FirstOrDefaultAsync();
            if (setting == null)
                return ApiResponse<SiteSettingDto>.Fail("Site setting chưa được khởi tạo.");

            _mapper.Map(req, setting);
            await _db.SaveChangesAsync();

            return ApiResponse<SiteSettingDto>.Ok(_mapper.Map<SiteSettingDto>(setting), "Đã cập nhật cấu hình website.");
        }

        // ===== UPLOAD LOGO / FAVICON =====
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file, [FromQuery] string type = "logo")
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { error = "File không hợp lệ" });

            try
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "settings");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{type}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var url = $"/uploads/settings/{fileName}";
                return Ok(new { location = url });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Upload setting file failed");
                return StatusCode(500, new { error = "Lỗi khi upload file" });
            }
        }
    }
}
