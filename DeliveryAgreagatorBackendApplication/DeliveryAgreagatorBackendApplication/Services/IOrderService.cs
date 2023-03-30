using DeliveryAgreagatorBackendApplication.Models.DTO;

namespace DeliveryAgreagatorBackendApplication.Services
{
    public interface IOrderService
    {
        public Task PostOrder(OrderPostDTO model, Guid userId);
        public Task CancelOrder(Guid orderId, Guid userId);
        public Task<List<OrderDTO>> GetActiveOrders(Guid userId);
        public Task<List<OrderDTO>> GetAllOrders(int page, Guid userId, bool? filterDateAsc, string? number);
        public Task RepeatOrder(Guid orderId, Guid userId);
    }
}
