using InteriorShop.Domain.Common;
using InteriorShop.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorShop.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string Code { get; set; } = default!;
        public string CustomerName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;

        //Địa chỉ ship
        public string AddressLine { get; set; } = default!;
        public string Ward { get; set; } = default!;
        public string District { get; set; } = default!;
        public string Province { get; set; } = default!;

        public string? Note { get; set; }

        //User (Nếu đã đăng ký tài khoản)
        public Guid? UserId { get; set; }

        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.COD;
        public OrderStatus Status { get; set; } = OrderStatus.New;

        public decimal Subtotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Total { get; set; }

        // Bank transfer info (for convenience in confirmation email)
        public string? BankAccountName { get; set; }
        public string? BankName { get; set; }
        public string? BankAccountNumber { get; set; }
        public string? TransferContent { get; set; }
    }
}
