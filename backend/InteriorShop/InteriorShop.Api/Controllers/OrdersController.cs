using AutoMapper;
using InteriorShop.Application.Common;
using InteriorShop.Application.DTOs.Orders;
using InteriorShop.Application.Requests.Orders;
using InteriorShop.Domain.Entities;
using InteriorShop.Domain.Enums;
using InteriorShop.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InteriorShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(AppDbContext db, IMapper mapper, ILogger<OrdersController> logger)
        {
            _db = db;
            _mapper = mapper;
            _logger = logger;
        }

        // ===== CREATE =====
        [HttpPost]
        public async Task<ActionResult<ApiResponse<OrderDto>>> Create([FromBody] OrderCreateRequest req)
        {
            // Lấy phí ship mặc định từ SiteSetting
            var shipFee = await _db.SiteSettings.Select(s => s.DefaultShippingFee).FirstOrDefaultAsync();

            var order = _mapper.Map<Order>(req);
            order.Code = "DH" + DateTime.UtcNow.Ticks;
            order.Status = OrderStatus.New;
            order.ShippingFee = shipFee;

            // Build items + check tồn kho
            foreach (var item in order.Items)
            {
                var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                if (product == null)
                    return ApiResponse<OrderDto>.Fail("Sản phẩm không tồn tại.");

                if (item.ProductVariantId.HasValue)
                {
                    var variant = await _db.ProductVariants
                        .FirstOrDefaultAsync(v => v.Id == item.ProductVariantId.Value && v.ProductId == product.Id);

                    if (variant == null)
                        return ApiResponse<OrderDto>.Fail("Biến thể sản phẩm không tồn tại.");

                    if (!variant.IsActive)
                        return ApiResponse<OrderDto>.Fail("Biến thể đã tắt bán.");

                    if (variant.Stock < item.Quantity)
                        return ApiResponse<OrderDto>.Fail($"Kho biến thể không đủ. Còn: {variant.Stock}");

                    variant.Stock -= item.Quantity;
                }
                else
                {
                    if (product.Stock.HasValue)
                    {
                        if (product.Stock.Value < item.Quantity)
                            return ApiResponse<OrderDto>.Fail($"Kho sản phẩm không đủ. Còn: {product.Stock.Value}");

                        product.Stock = product.Stock.Value - item.Quantity;
                    }
                }

                // snapshot giá
                item.ProductName = product.Name;
                item.ProductSlug = product.Slug;
                item.UnitPrice = item.UnitPrice == 0 ? (product.SalePrice ?? product.Price) : item.UnitPrice;
                item.LineTotal = item.UnitPrice * item.Quantity;
            }

            order.Subtotal = order.Items.Sum(x => x.LineTotal);
            order.Total = order.Subtotal + order.ShippingFee;

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            return ApiResponse<OrderDto>.Ok(_mapper.Map<OrderDto>(order));
        }

        // ===== GET (paged + filter) =====
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<OrderDto>>>> GetAll(
            [FromQuery] string? keyword,
            [FromQuery] OrderStatus? status,
            [FromQuery] DateTime? dateFrom,
            [FromQuery] DateTime? dateTo,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var q = _db.Orders.Include(o => o.Items).AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                q = q.Where(o =>
                    o.Code.Contains(keyword) ||
                    o.CustomerName.Contains(keyword) ||
                    o.Email.Contains(keyword) ||
                    o.Phone.Contains(keyword));
            }

            if (status.HasValue) q = q.Where(o => o.Status == status.Value);
            if (dateFrom.HasValue) q = q.Where(o => o.CreatedAt >= dateFrom.Value);
            if (dateTo.HasValue) q = q.Where(o => o.CreatedAt <= dateTo.Value);

            q = q.OrderByDescending(o => o.CreatedAt);

            var total = await q.CountAsync();
            var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return ApiResponse<PagedResult<OrderDto>>.Ok(new PagedResult<OrderDto>
            {
                Items = _mapper.Map<List<OrderDto>>(items),
                TotalItems = total,
                Page = page,
                PageSize = pageSize
            });
        }

        // ===== GET BY ID =====
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<OrderDto>>> GetById(Guid id)
        {
            var order = await _db.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return ApiResponse<OrderDto>.Fail("Không tìm thấy đơn hàng.");

            return ApiResponse<OrderDto>.Ok(_mapper.Map<OrderDto>(order));
        }

        // ===== UPDATE STATUS =====
        [HttpPut("{id:guid}/status")]
        public async Task<ActionResult<ApiResponse<OrderDto>>> UpdateStatus(Guid id, [FromBody] UpdateOrderStatusRequest req)
        {
            var order = await _db.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return ApiResponse<OrderDto>.Fail("Không tìm thấy đơn hàng.");

            // Nếu hủy -> hoàn kho
            if (req.Status == OrderStatus.Cancelled && order.Status != OrderStatus.Cancelled)
            {
                foreach (var i in order.Items)
                {
                    if (i.ProductVariantId.HasValue)
                    {
                        var v = await _db.ProductVariants.FirstOrDefaultAsync(x => x.Id == i.ProductVariantId.Value);
                        if (v != null) v.Stock += i.Quantity;
                    }
                    else
                    {
                        var p = await _db.Products.FirstOrDefaultAsync(x => x.Id == i.ProductId);
                        if (p != null && p.Stock.HasValue)
                            p.Stock = (p.Stock ?? 0) + i.Quantity;
                    }
                }
            }

            order.Status = req.Status;
            await _db.SaveChangesAsync();

            return ApiResponse<OrderDto>.Ok(_mapper.Map<OrderDto>(order));
        }

        // ===== DELETE =====
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(Guid id)
        {
            var order = await _db.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return ApiResponse<string>.Fail("Không tìm thấy đơn hàng.");

            // Nếu chưa hủy thì khi xóa phải hoàn kho
            if (order.Status != OrderStatus.Cancelled)
            {
                foreach (var i in order.Items)
                {
                    if (i.ProductVariantId.HasValue)
                    {
                        var v = await _db.ProductVariants.FirstOrDefaultAsync(x => x.Id == i.ProductVariantId.Value);
                        if (v != null) v.Stock += i.Quantity;
                    }
                    else
                    {
                        var p = await _db.Products.FirstOrDefaultAsync(x => x.Id == i.ProductId);
                        if (p != null && p.Stock.HasValue)
                            p.Stock = (p.Stock ?? 0) + i.Quantity;
                    }
                }
            }

            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();
            return ApiResponse<string>.Ok("Đã xóa đơn hàng.");
        }
    }
}
