using AutoMapper;
using InteriorShop.Application.Common;
using InteriorShop.Application.DTOs.Banners;
using InteriorShop.Application.Requests.Banners;
using InteriorShop.Domain.Entities;
using InteriorShop.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InteriorShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CampaignBannersController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public CampaignBannersController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<CampaignBannerDto>>>> GetAll()
        {
            var list = await _db.CampaignBanners.OrderBy(x => x.SortOrder).ToListAsync();
            return ApiResponse<List<CampaignBannerDto>>.Ok(_mapper.Map<List<CampaignBannerDto>>(list));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<CampaignBannerDto>>> GetById(Guid id)
        {
            var item = await _db.CampaignBanners.FindAsync(id);
            if (item == null) return ApiResponse<CampaignBannerDto>.Fail("Banner không tìm thấy!");
            return ApiResponse<CampaignBannerDto>.Ok(_mapper.Map<CampaignBannerDto>(item));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<CampaignBannerDto>>> Create([FromBody] CampaignBannerCreateRequest req)
        {
            var entity = _mapper.Map<CampaignBanner>(req);
            _db.CampaignBanners.Add(entity);
            await _db.SaveChangesAsync();

            return ApiResponse<CampaignBannerDto>.Ok(_mapper.Map<CampaignBannerDto>(entity));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ApiResponse<CampaignBannerDto>>> Update(Guid id, [FromBody] CampaignBannerUpdateRequest req)
        {
            var entity = await _db.CampaignBanners.FindAsync(id);
            if (entity == null) return ApiResponse<CampaignBannerDto>.Fail("Banner không tìm thấy!");

            _mapper.Map(req, entity);
            await _db.SaveChangesAsync();

            return ApiResponse<CampaignBannerDto>.Ok(_mapper.Map<CampaignBannerDto>(entity));
        }

        [HttpPut("{id:guid}/toogle")]
        public async Task<ActionResult<ApiResponse<CampaignBannerDto>>> Toggle(Guid id, [FromBody] bool isActive)
        {
            var banner = await _db.CampaignBanners.FindAsync(id);
            if (banner == null) return ApiResponse<CampaignBannerDto>.Fail("Banner không tìm thấy!");

            banner.IsActive = isActive;
            await _db.SaveChangesAsync();

            return ApiResponse<CampaignBannerDto>.Ok(_mapper.Map<CampaignBannerDto>(banner));
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(Guid id)
        {
            var entity = await _db.CampaignBanners.FindAsync(id);
            if (entity == null) return ApiResponse<string>.Fail("Banner không tìm thấy!");

            _db.CampaignBanners.Remove(entity);
            await _db.SaveChangesAsync();
            return ApiResponse<string>.Ok("Đã xóa Banner thành công!");
        }
    }
}
