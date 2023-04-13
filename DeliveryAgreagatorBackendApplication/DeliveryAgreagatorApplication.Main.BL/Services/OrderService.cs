using DeliveryAgreagatorApplication.API.Common.Models.DTO;
using DeliveryAgreagatorApplication.API.Common.Models.Enums;
using DeliveryAgreagatorApplication.Main.Common.Interfaces;
using DeliveryAgreagatorApplication.Main.DAL;
using DeliveryAgreagatorBackendApplication.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace DeliveryAgreagatorApplication.Main.BL.Services
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

        public async Task<List<OrderDTO>> GetActiveOrders(Guid userId) //TODO: удалить метод, если не понадобиться
        {
            var orders = _context.Orders.Include(x=>x.DishesInCart).ThenInclude(c=>c.Dish).ThenInclude(z=>z.Ratings).Where(x=>x.Status!=Status.Canceled && x.Status!=Status.Delivered && x.CustomerId==userId).ToList();
            var ordersDTO = orders.Select(x => x.ConvertToDTO()).ToList();
            return ordersDTO;
        }

        public async Task<List<OrderDTO>> GetAllOrders(int page, Guid userId, DateTime startDate, DateTime endDate,bool active, int? number)
        {
            if (number != null)
            {
                _regexp = number.ToString();
            }
            var orders = _context.Orders.Include(c=>c.DishesInCart).ThenInclude(c => c.Dish).ThenInclude(z => z.Ratings).Where(
            c => Regex.IsMatch(c.Id.ToString(), _regexp) && c.CustomerId==userId && c.OrderTime<=endDate && c.OrderTime>=startDate && (active ? (c.Status != Status.Canceled && c.Status != Status.Delivered) : (c.Status==Status.Canceled || c.Status==Status.Delivered))).ToList();
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
            var ordersDTO = orders.Select(x=>x.ConvertToDTO()).ToList();
            return ordersDTO;
        }

        public async Task PostOrder(OrderPostDTO model, Guid userId)
        {
            var guid = Guid.NewGuid();
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
                Id = guid,
                RestaurantId= dishesInCart[0].Dish.RestaurantId,
                CustomerId = userId,
                DishesInCart = dishesInCart,
                Number = guid.GetHashCode(),     
                OrderTime = model.OrderTime,
                Address = model.Address,
                Status = Status.Created,
                Price = dishesInCart.Sum(x=>x.Dish.Price*x.Counter)
            };
            foreach (var dish in dishesInCart)
            {
                dish.Active = false;
                dish.PriceWhenOpdered = dish.Dish.Price;
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

        public async Task<List<OrderDTO>> GetOrdersAvaliableToCook(DateSort? sort, int page, Guid cookId)
        {
            var cook = await _context.Cooks.FindAsync(cookId);
            var orders = _context.Orders.Include(x => x.DishesInCart).ThenInclude(x => x.Dish).Where(c => c.Status == Status.Created && c.RestaurantId == cook.RestaurantId).ToList(); 

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
           
            if(sort!=null) 
            {
                if (sort == DateSort.CreationAsc)
                {
                    orders = orders.OrderBy(x => x.OrderTime).ToList();
                }
                if (sort == DateSort.CreationDesc)
                {
                    orders = orders.OrderByDescending(x => x.OrderTime).ToList();
                }
                if (sort == DateSort.DeliveryAsc)
                {
                    orders = orders.OrderBy(x => x.DeliveryTime).ToList();
                }
                if (sort == DateSort.DeliveryDesc)
                {
                    orders = orders.OrderByDescending(x => x.DeliveryTime).ToList();
                }
            }
            orders = orders.Skip(_pageSize * (page - 1)).Take(_pageSize).ToList();
            var ordersDTO = orders.Select(x => x.ConvertToDTO()).ToList();
            return ordersDTO;
        }

        public async Task TakeOrderCook(Guid orderId, bool take, Guid cookId)
        {
            if (take)
            {
                var cook = await _context.Cooks.FindAsync(cookId);
                var order = await _context.Orders.Include(x => x.DishesInCart).ThenInclude(x => x.Dish).FirstOrDefaultAsync(c => c.Id == orderId && c.RestaurantId == cook.RestaurantId);
                if (order == null)
                {
                    throw new ArgumentException($"You cannot take order with this ${orderId} id!");
                }
                order.Status = Status.Kitchen;
                order.CookId = cookId;
            }
            else
            {
                var order = await _context.Orders.FirstOrDefaultAsync(x=>x.Id== orderId && x.CookId==cookId && (x.Status==Status.Kitchen || x.Status == Status.Packaging));
                if (order == null)
                {
                    throw new ArgumentException($"You haven't got order in progress with this ${orderId} id!");
                }
                if (order.Status == Status.Kitchen)
                {
                    order.Status = Status.Packaging;
                }
                else
                {
                    order.Status = Status.Packed;
                }
            }
            await _context.SaveChangesAsync();

        }

        public async Task<List<OrderDTO>> GetCookOrdersStory(int? number, int page, Guid cookId)
        {
            if (number != null)
            {
                _regexp = number.ToString();
            }
            var orders = _context.Orders.Include(c => c.DishesInCart).ThenInclude(c => c.Dish).Where(
            c => Regex.IsMatch(c.Number.ToString(), _regexp) && c.CookId == cookId && c.Status!=Status.Kitchen && c.Status!=Status.Packaging);
            var ordersDTO = orders.Select(x => x.ConvertToDTO()).ToList();
            return ordersDTO;
        }

        public async Task<List<OrderDTO>> GetOrdersAvaliableToCourier(Guid coourierId)
        {
            var orders = _context.Orders.Where(x => x.Status == Status.Packed);
            var ordersDTO = orders.Select(x=>x.ConvertToDTO()).ToList() ;
            return ordersDTO;
        }

        public async Task TakeOrderCourier(Guid orderId, bool take, Guid courierId)
        {
            if (take)
            {
                var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId && x.Status == Status.Packed);
                if (order == null)
                {
                    throw new InvalidOperationException($"You can't take order with this {orderId} id!"); //TODO: сделать более точные исключения
                }
                order.DeliveryTime = DateTime.UtcNow.AddHours(1);
                order.Status = Status.Delivery;
                order.CourierId = courierId;
            }
            else
            {
                var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId && x.Status == Status.Delivery && x.CourierId==courierId);
                if (order == null)
                {
                    throw new InvalidOperationException($"You can't modify order with this {orderId} id!"); //TODO: сделать более точные исключения
                }
                order.Status = Status.Delivered;
            }
            await _context.SaveChangesAsync(); 
        }

        public async Task CancelOrderCourier(Guid orderId, Guid courierId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId && x.CourierId == courierId);
            if (order == null)
            {
                throw new ArgumentException($"You haven't got order with this {orderId} Id!");
            }
            if (order.Status != Status.Delivery)
            {
                throw new InvalidOperationException($"You cant cancel order with this {order.Status.ToString()}");
            }
            order.Status = Status.Canceled;
            await _context.SaveChangesAsync();
        }

        public async Task<List<OrderDTO>> GetRestaurantOrders(int page, Guid managerId, DateTime startDateOrder, DateTime endDateOrder, DateTime startDateDelivery, DateTime endDateDelivery, int? number)
        {
            if (number != null)
            {
                _regexp = number.ToString();
            }
            var manager = await _context.Managers.FindAsync(managerId);
            var orders = _context.Orders.Include(x=>x.DishesInCart).ThenInclude(x=>x.Dish).Where(x => Regex.IsMatch(x.Number.ToString(), _regexp) && 
            x.RestaurantId == manager.RestaurantId && x.OrderTime <= endDateOrder && x.OrderTime >= startDateOrder &&
            (x.DeliveryTime == null || (x.DeliveryTime<=endDateDelivery && x.DeliveryTime >= startDateDelivery)));

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
            orders = orders.Skip(_pageSize * (page - 1)).Take(_pageSize);
            var ordersDTO = orders.Select(x => x.ConvertToDTO()).ToList();
            return ordersDTO;
        }
    }
}
