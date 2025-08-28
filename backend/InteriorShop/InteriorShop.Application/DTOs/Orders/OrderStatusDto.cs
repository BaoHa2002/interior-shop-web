using InteriorShop.Domain.Enums;

namespace InteriorShop.Application.DTOs.Orders
{
    public class OrderStatusDto
    {
        public Guid OrderId { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}