using AutoMapper;
using InteriorShop.Application.Common;
using InteriorShop.Application.DTOs.Option;
using InteriorShop.Application.Requests.Options;
using InteriorShop.Domain.Entities;
using InteriorShop.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InteriorShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OptionsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public OptionsController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        // ===== GET ALL GROUPS =====
        [HttpGet("groups")]
        public async Task<ActionResult<ApiResponse<List<OptionGroupDto>>>> GetGroups()
        {
            var groups = await _db.OptionGroups.Include(g => g.Values).ToListAsync();
            return ApiResponse<List<OptionGroupDto>>.Ok(_mapper.Map<List<OptionGroupDto>>(groups));
        }

        // ===== GET GROUP BY ID =====
        [HttpGet("groups/{id:guid}")]
        public async Task<ActionResult<ApiResponse<OptionGroupDto>>> GetGroup(Guid id)
        {
            var group = await _db.OptionGroups.Include(g => g.Values).FirstOrDefaultAsync(g => g.Id == id);
            if (group == null) return ApiResponse<OptionGroupDto>.Fail("Không tìm thấy nhóm option.");
            return ApiResponse<OptionGroupDto>.Ok(_mapper.Map<OptionGroupDto>(group));
        }

        // ===== CREATE GROUP =====
        [HttpPost("groups")]
        public async Task<ActionResult<ApiResponse<OptionGroupDto>>> CreateGroup([FromBody] OptionGroupCreateRequest req)
        {
            var group = _mapper.Map<OptionGroup>(req);
            _db.OptionGroups.Add(group);
            await _db.SaveChangesAsync();
            return ApiResponse<OptionGroupDto>.Ok(_mapper.Map<OptionGroupDto>(group));
        }

        // ===== UPDATE GROUP =====
        [HttpPut("groups/{id:guid}")]
        public async Task<ActionResult<ApiResponse<OptionGroupDto>>> UpdateGroup(Guid id, [FromBody] OptionGroupUpdateRequest req)
        {
            var group = await _db.OptionGroups.Include(g => g.Values).FirstOrDefaultAsync(g => g.Id == id);
            if (group == null) return ApiResponse<OptionGroupDto>.Fail("Không tìm thấy nhóm option.");

            _mapper.Map(req, group);
            await _db.SaveChangesAsync();
            return ApiResponse<OptionGroupDto>.Ok(_mapper.Map<OptionGroupDto>(group));
        }

        // ===== DELETE GROUP =====
        [HttpDelete("groups/{id:guid}")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteGroup(Guid id)
        {
            var group = await _db.OptionGroups.Include(g => g.Values).FirstOrDefaultAsync(g => g.Id == id);
            if (group == null) return ApiResponse<string>.Fail("Không tìm thấy nhóm option.");

            _db.OptionValues.RemoveRange(group.Values);
            _db.OptionGroups.Remove(group);
            await _db.SaveChangesAsync();
            return ApiResponse<string>.Ok("Đã xóa nhóm option.");
        }

        // ===== CREATE VALUE =====
        [HttpPost("values")]
        public async Task<ActionResult<ApiResponse<OptionValueDto>>> CreateValue([FromBody] OptionValueCreateRequest req)
        {
            var value = _mapper.Map<OptionValue>(req);
            _db.OptionValues.Add(value);
            await _db.SaveChangesAsync();
            return ApiResponse<OptionValueDto>.Ok(_mapper.Map<OptionValueDto>(value));
        }

        // ===== DELETE VALUE =====
        [HttpDelete("values/{id:guid}")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteValue(Guid id)
        {
            var value = await _db.OptionValues.FirstOrDefaultAsync(v => v.Id == id);
            if (value == null) return ApiResponse<string>.Fail("Không tìm thấy giá trị option.");

            _db.OptionValues.Remove(value);
            await _db.SaveChangesAsync();
            return ApiResponse<string>.Ok("Đã xóa giá trị option.");
        }
    }
}
