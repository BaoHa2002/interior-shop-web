using InteriorShop.Domain.Enums;

namespace InteriorShop.Application.Requests.Orders
{
    public class OrderCreateRequest
    {
        // Customer
        public string CustomerName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;

        // Shipping address
        public string AddressLine { get; set; } = default!;
        public string Ward { get; set; } = default!;
        public string District { get; set; } = default!;
        public string Province { get; set; } = default!;
        public string? Note { get; set; }

        // Optional: user id (nếu khách đã đăng ký)
        public Guid? UserId { get; set; }

        // Payment & Items
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.COD;
        public List<OrderItemRequest> Items { get; set; } = new();
    }
}