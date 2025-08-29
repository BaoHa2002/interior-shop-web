using AutoMapper;
using InteriorShop.Application.Common;
using InteriorShop.Application.DTOs.Products;
using InteriorShop.Application.Requests.Products;
using InteriorShop.Domain.Entities;
using InteriorShop.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InteriorShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public ProductsController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<ProductDto>>>> GetAll([FromQuery] ProductQueryRequest req)
        {
            var query = _db.Products.Include(p => p.Images).AsQueryable();

            if (!string.IsNullOrEmpty(req.Keyword))
                query = query.Where(p => p.Name.Contains(req.Keyword));

            if (req.CategoryId.HasValue)
                query = query.Where(p => p.Categories.Any(c => c.Id == req.CategoryId));

            if (req.IsActive.HasValue)
                query = query.Where(p => p.IsActive == req.IsActive);

            if (req.IsFeatured.HasValue)
                query = query.Where(p => p.IsFeatured == req.IsFeatured);

            if (req.MinPrice.HasValue)
                query = query.Where(p => p.Price >= req.MinPrice.Value);

            if (req.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= req.MaxPrice.Value);

            // Sort
            query = req.SortBy switch
            {
                "name_asc" => query.OrderBy(p => p.Name),
                "name_desc" => query.OrderByDescending(p => p.Name),
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "newest" => query.OrderByDescending(p => p.PublishedAt),
                _ => query.OrderByDescending(p => p.CreatedAt)
            };

            var total = await query.CountAsync();
            var items = await query.Skip((req.Page - 1) * req.PageSize)
                                   .Take(req.PageSize)
                                   .ToListAsync();

            return ApiResponse<PagedResult<ProductDto>>.Ok(new PagedResult<ProductDto>
            {
                Items = _mapper.Map<List<ProductDto>>(items),
                TotalItems = total,
                Page = req.Page,
                PageSize = req.PageSize
            });
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> GetById(Guid id)
        {
            var product = await _db.Products
                .Include(p => p.Id)
                .Include(p => p.Variants)
                .Include(p => p.OptionsGroups)
                    .ThenInclude(g => g.Values)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return ApiResponse<ProductDto>.Fail("Product not found");

            return ApiResponse<ProductDto>.Ok(_mapper.Map<ProductDto>(product));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ProductDto>>> Create([FromBody] ProductCreateRequest req)
        {
            var product = _mapper.Map<Product>(req);
            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            return ApiResponse<ProductDto>.Ok(_mapper.Map<ProductDto>(product));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> Update(Guid id, [FromBody] ProductUpdateRequest req)
        {
            var product = await _db.Products
                .Include(p => p.Images)
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return ApiResponse<ProductDto>.Fail("Product not found");

            _mapper.Map(req, product);
            await _db.SaveChangesAsync();

            return ApiResponse<ProductDto>.Ok(_mapper.Map<ProductDto>(product));
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(Guid id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null)
                return ApiResponse<string>.Fail("Product not found");

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            return ApiResponse<string>.Ok("Deleted successfully");
        }
    }
}
