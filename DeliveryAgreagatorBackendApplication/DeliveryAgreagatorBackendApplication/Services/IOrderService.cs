using DeliveryAgreagatorBackendApplication.Model.Enums;
using DeliveryAgreagatorBackendApplication.Models.DTO;

namespace DeliveryAgreagatorBackendApplication.Services
{
    public interface IOrderService
    {
        public Task PostOrder(OrderPostDTO model, Guid userId);
        public Task CancelOrder(Guid orderId, Guid userId);
        public Task<List<OrderDTO>> GetActiveOrders(Guid userId);
        public Task<List<OrderDTO>> GetAllOrders(int page, Guid userId, DateTime startDate, DateTime endDate, int? number);
        public Task RepeatOrder(Guid orderId, Guid userId);
        public Task<List<OrderDTO>> GetOrdersAvaliableToCook(DateSort? sort, int page, Guid cookId);
        public Task<List<OrderDTO>> GetCookOrdersStory(int? number, int page, Guid cookId);
        public Task TakeOrderCook(Guid orderId, bool take, Guid cookId);
        public Task<List<OrderDTO>> GetOrdersAvaliableToCourier(Guid courierId);
        public Task TakeOrderCourier(Guid orderId,Guid courierId);
        public Task CancelOrderCourier(Guid orderId, Guid courierId);
        public Task<List<OrderDTO>> GetRestaurantOrders(int page, Guid managerId, DateTime startDateOrder, DateTime endDateOrder, DateTime startDateDelivery, DateTime endDateDelivery, int? number);
    }
}
