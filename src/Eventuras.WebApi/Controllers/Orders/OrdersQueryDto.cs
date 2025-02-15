using Eventuras.Domain;
using Eventuras.WebApi.Models;

namespace Eventuras.WebApi.Controllers.Orders
{
    public class OrdersQueryDto : PageQueryDto
    {
        public string UserId { get; set; }

        public int? EventId { get; set; }

        public int? RegistrationId { get; set; }

        public Order.OrderStatus? Status { get; set; }

        public bool IncludeUser { get; set; }

        public bool IncludeRegistration { get; set; }
    }
}
