using AutoMapper;
using InteriorShop.Application.Common;
using InteriorShop.Application.DTOs.Category;
using InteriorShop.Application.Requests.Categories;
using InteriorShop.Domain.Entities;
using InteriorShop.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InteriorShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public CategoryController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<CategoryDto>>>> GetAll()
        {
            var categories = await _db.Categories
                .Include(c => c.Children)
                .ToListAsync();

            return ApiResponse<List<CategoryDto>>.Ok(_mapper.Map<List<CategoryDto>>(categories));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> GetById(Guid id)
        {
            var category = await _db.Categories
                .Include(c => c.Children)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return ApiResponse<CategoryDto>.Fail("Danh mục không tìm thấy!");

            return ApiResponse<CategoryDto>.Ok(_mapper.Map<CategoryDto>(category));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> Create([FromBody] CategoryCreateRequest req)
        {
            var category = _mapper.Map<Category>(req);
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();

            return ApiResponse<CategoryDto>.Ok(_mapper.Map<CategoryDto>(category));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> Update(Guid id, [FromBody] CategoryUpdateRequest req)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null)
                return ApiResponse<CategoryDto>.Fail("Danh mục không tìm thấy!");

            _mapper.Map(req, category);
            await _db.SaveChangesAsync();

            return ApiResponse<CategoryDto>.Ok(_mapper.Map<CategoryDto>(category));
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(Guid id)
        {
            var category = await _db.Categories
                .Include(c => c.Children)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return ApiResponse<string>.Fail("Danh mục không tìm thấy!");

            if (category.Children.Any())
                return ApiResponse<string>.Fail("Không thể xóa danh mục có danh mục con");

            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();

            return ApiResponse<string>.Ok("Đã xóa danh mục thành công");
        }
    }
}
