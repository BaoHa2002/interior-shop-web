using ClosedXML.Excel;
using InteriorShop.Application.Common;
using InteriorShop.Domain.Entities;
using InteriorShop.Domain.Enums;
using InteriorShop.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InteriorShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExportController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ILogger<ExportController> _logger;

        public ExportController(AppDbContext db, ILogger<ExportController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // ===== 1. Xuất khách hàng liên hệ =====
        [HttpGet("contacts")]
        public async Task<IActionResult> ExportContacts()
        {
            var contacts = await _db.Contacts.OrderByDescending(c => c.CreatedAt).ToListAsync();

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("KhachHangLienHe");

            // Header
            ws.Cell(1, 1).Value = "Họ tên khách hàng";
            ws.Cell(1, 2).Value = "Email";
            ws.Cell(1, 3).Value = "Số điện thoại";
            ws.Cell(1, 4).Value = "Nội dung liên hệ";
            ws.Cell(1, 5).Value = "Ngày gửi";

            // Rows
            for (int i = 0; i < contacts.Count; i++)
            {
                var c = contacts[i];
                ws.Cell(i + 2, 1).Value = c.FullName;
                ws.Cell(i + 2, 2).Value = c.Email;
                ws.Cell(i + 2, 3).Value = c.Phone;
                ws.Cell(i + 2, 4).Value = c.Message;
                ws.Cell(i + 2, 5).Value = c.CreatedAt.ToString("dd/MM/yyyy HH:mm");
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"DanhSachLienHe_{DateTime.Now:yyyyMMdd}.xlsx");
        }

        // ===== 2. Xuất danh sách đơn hàng =====
        [HttpGet("orders")]
        public async Task<IActionResult> ExportOrders()
        {
            var orders = await _db.Orders
                .Include(o => o.Items).ThenInclude(i => i.Product)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("DonHang");

            ws.Cell(1, 1).Value = "Mã đơn";
            ws.Cell(1, 2).Value = "Họ tên khách hàng";
            ws.Cell(1, 3).Value = "Email";
            ws.Cell(1, 4).Value = "Số điện thoại";
            ws.Cell(1, 5).Value = "Ngày đặt";
            ws.Cell(1, 6).Value = "Tổng tiền";
            ws.Cell(1, 7).Value = "Trạng thái";

            for (int i = 0; i < orders.Count; i++)
            {
                var o = orders[i];
                ws.Cell(i + 2, 1).Value = o.Id;
                ws.Cell(i + 2, 2).Value = o.CustomerName;
                ws.Cell(i + 2, 3).Value = o.Email;
                ws.Cell(i + 2, 4).Value = o.Phone;
                ws.Cell(i + 2, 5).Value = o.CreatedAt.ToString("dd/MM/yyyy");
                ws.Cell(i + 2, 6).Value = o.Total;
                ws.Cell(i + 2, 7).Value = o.Status.ToString();
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"DanhSachDonHang_{DateTime.Now:yyyyMMdd}.xlsx");
        }

        // ===== 3. Xuất sản phẩm bán ra =====
        [HttpGet("products-sold")]
        public async Task<IActionResult> ExportProductsSold()
        {
            var items = await _db.OrderItems
                .Include(i => i.Product)
                .ToListAsync();

            var grouped = items.GroupBy(i => i.Product.Name)
                .Select(g => new
                {
                    TenSanPham = g.Key,
                    SoLuong = g.Sum(x => x.Quantity),
                    TongTien = g.Sum(x => x.Quantity * x.UnitPrice)
                }).ToList();

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("SanPhamBanRa");

            ws.Cell(1, 1).Value = "Tên sản phẩm";
            ws.Cell(1, 2).Value = "Số lượng bán";
            ws.Cell(1, 3).Value = "Tổng tiền";

            for (int i = 0; i < grouped.Count; i++)
            {
                var g = grouped[i];
                ws.Cell(i + 2, 1).Value = g.TenSanPham;
                ws.Cell(i + 2, 2).Value = g.SoLuong;
                ws.Cell(i + 2, 3).Value = g.TongTien;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"SanPhamBanRa_{DateTime.Now:yyyyMMdd}.xlsx");
        }

        // ===== 4. Báo cáo doanh thu theo tháng =====
        [HttpGet("revenue-monthly")]
        public async Task<IActionResult> ExportRevenueMonthly(int year)
        {
            var orders = await _db.Orders
                .Where(o => o.CreatedAt.Year == year && o.Status == OrderStatus.Completed)
                .ToListAsync();

            var grouped = orders.GroupBy(o => o.CreatedAt.Month)
                .Select(g => new
                {
                    Thang = g.Key,
                    TongDonHang = g.Count(),
                    TongTien = g.Sum(x => x.Total)
                }).ToList();

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("DoanhThuTheoThang");

            ws.Cell(1, 1).Value = "Tháng";
            ws.Cell(1, 2).Value = "Số đơn hàng";
            ws.Cell(1, 3).Value = "Tổng doanh thu";

            for (int i = 0; i < grouped.Count; i++)
            {
                var g = grouped[i];
                ws.Cell(i + 2, 1).Value = g.Thang;
                ws.Cell(i + 2, 2).Value = g.TongDonHang;
                ws.Cell(i + 2, 3).Value = g.TongTien;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"DoanhThuThang_{year}.xlsx");
        }
    }
}
