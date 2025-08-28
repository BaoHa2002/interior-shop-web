using InteriorShop.Domain.Enums;

namespace InteriorShop.Application.Requests.Orders
{
    public class UpdateOrderStatusRequest
    {
        public Guid OrderId { get; set; }
        public OrderStatus Status { get; set; }
    }
}