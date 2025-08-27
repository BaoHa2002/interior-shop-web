using InteriorShop.Domain.Enums;

namespace InteriorShop.Application.DTOs.Orders
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = default!;
        public string CustomerName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;

        public string AddressLine { get; set; } = default!;
        public string Ward { get; set; } = default!;
        public string District { get; set; } = default!;
        public string Province { get; set; } = default!;

        public string? Note { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public OrderStatus Status { get; set; }

        public decimal Subtotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Total { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<OrderItemDto> Items { get; set; } = new();
    }
}