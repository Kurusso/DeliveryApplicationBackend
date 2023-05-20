using DeliveryAgreagatorApplication.API.Common.Models.DTO;
using DeliveryAgreagatorApplication.API.Common.Models.Enums;
using DeliveryAgreagatorApplication.Main.Common.Models.DTO;
using System.Security.Claims;

namespace DeliveryAgreagatorApplication.Main.Common.Interfaces
{
    public interface IOrderService
    {
        public Task PostOrder(OrderPostDTO model, ClaimsPrincipal userPrincipal);
        public Task CancelOrder(Guid orderId, Guid userId);
        public Task<List<OrderDTO>> GetAllOrders(int page, Guid userId, DateTime startDate, DateTime endDate,bool active, int? number);
        public Task RepeatOrder(Guid orderId, Guid userId);
        public Task<List<OrderDTO>> GetOrdersAvaliableToCook(bool active,DateSort? sort, int page, Guid cookId);
        public Task TakeOrderCook(Guid orderId, Guid cookId, StatusDTO status);
        public Task<List<OrderDTO>> GetOrdersAvaliableToCourier(Guid courierId, int page);
        public Task TakeOrderCourier(Guid orderId, Guid courierId, StatusDTO status);
        public Task CancelOrderCourier(Guid orderId, Guid courierId);
        public Task<List<OrderDTO>> GetRestaurantOrders(int page, Guid managerId, DateTime startDateOrder, DateTime endDateOrder, DateTime startDateDelivery, DateTime endDateDelivery, int? number);
    }
}
