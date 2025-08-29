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
    public class BannerSlidesController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public BannerSlidesController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        // GET all
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<BannerDto>>>> GetAll()
        {
            var banners = await _db.BannerSlides.OrderBy(x => x.SortOrder).ToListAsync();
            return ApiResponse<List<BannerDto>>.Ok(_mapper.Map<List<BannerDto>>(banners));
        }

        // GET by id
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<BannerDto>>> GetById(Guid id)
        {
            var banner = await _db.BannerSlides.FindAsync(id);
            if (banner == null) return ApiResponse<BannerDto>.Fail("Không tìm thấy Banner!");

            return ApiResponse<BannerDto>.Ok(_mapper.Map<BannerDto>(banner));
        }

        // CREATE
        [HttpPost]
        public async Task<ActionResult<ApiResponse<BannerDto>>> Create([FromBody] BannerCreateRequest request)
        {
            var banner = _mapper.Map<BannerSlide>(request);
            _db.BannerSlides.Add(banner);
            await _db.SaveChangesAsync();

            return ApiResponse<BannerDto>.Ok(_mapper.Map<BannerDto>(banner));
        }

        // UPDATE
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ApiResponse<BannerDto>>> Update(Guid id, [FromBody] BannerUpdateRequest request)
        {
            var banner = await _db.BannerSlides.FindAsync(id);
            if (banner == null) return ApiResponse<BannerDto>.Fail("Không tìm thấy Banner!");

            _mapper.Map(request, banner);
            await _db.SaveChangesAsync();

            return ApiResponse<BannerDto>.Ok(_mapper.Map<BannerDto>(banner));
        }

        // DELETE
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(Guid id)
        {
            var banner = await _db.BannerSlides.FindAsync(id);
            if (banner == null) return ApiResponse<string>.Fail("Không tìm thấy Banner!");

            _db.BannerSlides.Remove(banner);
            await _db.SaveChangesAsync();
            return ApiResponse<string>.Ok("Đã xóa Banner thành công!");
        }
    }
}
