using AutoMapper;
using InteriorShop.Application.Common;
using InteriorShop.Application.DTOs.Content;
using InteriorShop.Application.Requests.Content;
using InteriorShop.Domain.Entities;
using InteriorShop.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InteriorShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContentController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly ILogger<ContentController> _logger;

        public ContentController(AppDbContext db, IMapper mapper, ILogger<ContentController> logger)
        {
            _db = db;
            _mapper = mapper;
            _logger = logger;
        }

        // api/content/blogposts?keyword=sofa&page=1&pageSize=10
        [HttpGet("blogposts")]
        public async Task<ActionResult<ApiResponse<PagedResult<BlogPostDto>>>> GetAll(
            [FromQuery] string? keyword,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _db.BlogPosts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x => x.Title.Contains(keyword) || x.Slug.Contains(keyword));
            }

            query = query.OrderByDescending(x => x.CreatedAt);

            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var result = new PagedResult<BlogPostDto>
            {
                Items = _mapper.Map<List<BlogPostDto>>(items),
                TotalItems = total,
                Page = page,
                PageSize = pageSize
            };

            return ApiResponse<PagedResult<BlogPostDto>>.Ok(result);
        }

        [HttpGet("blogposts/{id:guid}")]
        public async Task<ActionResult<ApiResponse<BlogPostDto>>> GetById(Guid id)
        {
            var blog = await _db.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);
            if (blog == null) return ApiResponse<BlogPostDto>.Fail("Không tìm thấy bài viết");

            return ApiResponse<BlogPostDto>.Ok(_mapper.Map<BlogPostDto>(blog));
        }

        [HttpPost("blogposts")]
        public async Task<ActionResult<ApiResponse<BlogPostDto>>> Create([FromBody] BlogPostCreateRequest req)
        {
            if (await _db.BlogPosts.AnyAsync(x => x.Slug == req.Slug))
                return ApiResponse<BlogPostDto>.Fail("Slug đã tồn tại.");

            var blog = _mapper.Map<BlogPost>(req);
            blog.IsPublished = false; // mặc định chưa publish
            _db.BlogPosts.Add(blog);
            await _db.SaveChangesAsync();

            return ApiResponse<BlogPostDto>.Ok(_mapper.Map<BlogPostDto>(blog));
        }

        [HttpPut("blogposts/{id:guid}")]
        public async Task<ActionResult<ApiResponse<BlogPostDto>>> Update(Guid id, [FromBody] BlogPostUpdateRequest req)
        {
            var blog = await _db.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);
            if (blog == null) return ApiResponse<BlogPostDto>.Fail("Không tìm thấy bài viết.");

            // check slug trùng (ngoại trừ chính nó)
            if (await _db.BlogPosts.AnyAsync(x => x.Slug == req.Slug && x.Id != id))
                return ApiResponse<BlogPostDto>.Fail("Slug đã tồn tại.");

            _mapper.Map(req, blog);
            await _db.SaveChangesAsync();

            return ApiResponse<BlogPostDto>.Ok(_mapper.Map<BlogPostDto>(blog));
        }

        [HttpPut("blogposts/{id:guid}/publish")]
        public async Task<ActionResult<ApiResponse<BlogPostDto>>> Publish(Guid id, [FromQuery] bool publish = true)
        {
            var blog = await _db.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);
            if (blog == null) return ApiResponse<BlogPostDto>.Fail("Không tìm thấy bài viết.");

            blog.IsPublished = publish;
            if (publish && !blog.PublishedAt.HasValue)
                blog.PublishedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return ApiResponse<BlogPostDto>.Ok(_mapper.Map<BlogPostDto>(blog));
        }

        [HttpDelete("blogposts/{id:guid}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(Guid id)
        {
            var blog = await _db.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);
            if (blog == null) return ApiResponse<string>.Fail("Không tìm thấy bài viết.");

            _db.BlogPosts.Remove(blog);
            await _db.SaveChangesAsync();
            return ApiResponse<string>.Ok("Đã xóa bài viết.");
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { error = "File không hợp lệ" });

            try
            {
                // Tạo thư mục lưu ảnh: wwwroot/uploads/blog
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "blog");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // Đặt tên file duy nhất
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Lưu file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Trả về JSON cho TinyMCE (chú ý key phải là "location")
                var url = $"/uploads/blog/{fileName}";
                return Ok(new { location = url });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Upload image failed");
                return StatusCode(500, new { error = "Lỗi khi upload file" });
            }
        }
    }
}
