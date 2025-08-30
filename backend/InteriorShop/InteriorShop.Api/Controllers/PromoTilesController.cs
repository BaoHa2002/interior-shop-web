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
    public class PromoTilesController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public PromoTilesController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<PromoTileDto>>>> GetAll()
        {
            var list = await _db.PromoTiles.OrderBy(x => x.SortOrder).ToListAsync();
            return ApiResponse<List<PromoTileDto>>.Ok(_mapper.Map<List<PromoTileDto>>(list));
        }

        [HttpGet("active")]
        public async Task<ActionResult<ApiResponse<List<PromoTileDto>>>> GetActive()
        {
            var list = await _db.PromoTiles.Where(x => x.IsActive).OrderBy(x => x.SortOrder).ToListAsync();
            return ApiResponse<List<PromoTileDto>>.Ok(_mapper.Map<List<PromoTileDto>>(list));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<PromoTileDto>>> GetById(Guid id)
        {
            var item = await _db.PromoTiles.FindAsync(id);
            if (item == null) return ApiResponse<PromoTileDto>.Fail("Not found");
            return ApiResponse<PromoTileDto>.Ok(_mapper.Map<PromoTileDto>(item));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PromoTileDto>>> Create([FromBody] PromoTileCreateRequest req)
        {
            var entity = _mapper.Map<PromoTile>(req);
            _db.PromoTiles.Add(entity);
            await _db.SaveChangesAsync();

            return ApiResponse<PromoTileDto>.Ok(_mapper.Map<PromoTileDto>(entity));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ApiResponse<PromoTileDto>>> Update(Guid id, [FromBody] PromoTileUpdateRequest req)
        {
            var entity = await _db.PromoTiles.FindAsync(id);
            if (entity == null) return ApiResponse<PromoTileDto>.Fail("Not found");

            _mapper.Map(req, entity);
            await _db.SaveChangesAsync();

            return ApiResponse<PromoTileDto>.Ok(_mapper.Map<PromoTileDto>(entity));
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(Guid id)
        {
            var entity = await _db.PromoTiles.FindAsync(id);
            if (entity == null) return ApiResponse<string>.Fail("Not found");

            _db.PromoTiles.Remove(entity);
            await _db.SaveChangesAsync();
            return ApiResponse<string>.Ok("Deleted");
        }
    }
}
