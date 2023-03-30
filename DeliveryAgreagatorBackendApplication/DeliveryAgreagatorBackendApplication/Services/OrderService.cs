using DeliveryAgreagatorBackendApplication.Models;
using DeliveryAgreagatorBackendApplication.Models.DTO;
using DeliveryAgreagatorBackendApplication.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace DeliveryAgreagatorBackendApplication.Services
{
    public class OrderService : IOrderService
    {
        private readonly int _pageSize = 6;
        private int _pageCount = 6;
        private string _regexp = "";
        private readonly BackendDbContext _context;
        public OrderService(BackendDbContext context)
        {
            _context = context;
        }

        public async Task CancelOrder(Guid orderId, Guid userId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x=>x.Id==orderId && x.CustomerId==userId);
            if (order == null)
            {
                throw new ArgumentException($"You haven't got order with this {orderId} Id!");
            }
            if(order.Status!=Status.Created)
            {
                throw new InvalidOperationException($"You cant cancel order with this {order.Status.ToString()}");
            }
            order.Status = Status.Canceled;
            await _context.SaveChangesAsync();
        }

        public async Task<List<OrderDTO>> GetActiveOrders(Guid userId)
        {
            var orders = _context.Orders.Include(x=>x.DishesInCart).ThenInclude(c=>c.Dish).ThenInclude(z=>z.Ratings).Where(x=>x.Status!=Status.Canceled && x.Status!=Status.Delivered && x.CustomerId==userId).ToList();
            var ordersDTO = orders.Select(x => new OrderDTO(x)).ToList();
            return ordersDTO;
        }

        public async Task<List<OrderDTO>> GetAllOrders(int page, Guid userId, bool? filterDateAsc, string? number)
        {
            if (number != null)
            {
                _regexp = number;
            }
            var orders = _context.Orders.Include(c=>c.DishesInCart).ThenInclude(c => c.Dish).ThenInclude(z => z.Ratings).Where(
            c => Regex.IsMatch(c.Id.ToString(), _regexp) && c.CustomerId==userId).ToList();
            if(filterDateAsc!= null)
            {
                if (filterDateAsc.Value)
                {
                    orders=orders.OrderBy(x=>x.OrderTime).ToList();
                }
                else
                {
                    orders=orders.OrderByDescending(x=>x.OrderTime).ToList();
                }
            }
            if ((orders.Count() % _pageSize) == 0)
            {
                _pageCount = (orders.Count() / _pageSize);
            }
            else
            {
                _pageCount = (orders.Count() / _pageSize) + 1;
            }
            if (page > _pageCount || page <= 0)
            {
                throw new ArgumentOutOfRangeException($"{page} is incorrect page number!");
            }
            orders = orders.Skip(_pageSize * (page - 1)).Take(_pageSize).ToList();
            var ordersDTO = orders.Select(x=>new OrderDTO(x)).ToList();
            return ordersDTO;
        }

        public async Task PostOrder(OrderPostDTO model, Guid userId)
        {
            var dishesInCart = _context.DishInCart.Include(x=>x.Dish).Where(x => x.CustomerId == userId && x.Active).ToList();
            if (dishesInCart.Count == 0 )
            {
                throw new InvalidOperationException("Your cart is empty!");
            }
            if (dishesInCart.Any(x => x.Dish.RestaurantId != dishesInCart[0].Dish.RestaurantId)){
                throw new InvalidOperationException("You nant order dishes from different restaurants at the same time!"); //TODO: добавить кастомный эксепшн
            }
            var order = new OrderDbModel
            {
                Id = new Guid(),
                CustomerId = userId,
                DishesInCart = dishesInCart,
                     
                OrderTime = model.OrderTime,
                Address = model.Address,
                Status = Status.Created
            };
            foreach (var dish in dishesInCart)
            {
                dish.Active = false;
            }
            await _context.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task RepeatOrder(Guid orderId, Guid userId)
        {
            var order = await _context.Orders.Include(x=>x.DishesInCart).ThenInclude(c=>c.Dish).ThenInclude(z=>z.Ratings).FirstOrDefaultAsync(x=>x.Id==orderId && x.CustomerId==userId);
            if(order == null)
            {
                throw new ArgumentException($"You haven't got order with this {orderId} id!");
            }
            var unavalibleDish = order.DishesInCart.FirstOrDefault(x=>!x.Dish.IsActive);
            if(unavalibleDish != null)
            {
                throw new InvalidOperationException($"You can't repeat this order, because dish with {unavalibleDish.Id} id now is unavalible!");
            }
            var newOrder =  new OrderDbModel(order);
            await _context.AddAsync(newOrder);
            await _context.SaveChangesAsync();
        }
    }
}
