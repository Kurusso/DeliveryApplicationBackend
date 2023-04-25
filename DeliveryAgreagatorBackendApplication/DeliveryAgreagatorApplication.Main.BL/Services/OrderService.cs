using DeliveryAgreagatorApplication.API.Common.Models.DTO;
using DeliveryAgreagatorApplication.API.Common.Models.Enums;
using DeliveryAgreagatorApplication.Common.Exceptions;
using DeliveryAgreagatorApplication.Common.Models.Enums;
using DeliveryAgreagatorApplication.Common.Models.Notification;
using DeliveryAgreagatorApplication.Main.Common.Interfaces;
using DeliveryAgreagatorApplication.Main.Common.Models.DTO;
using DeliveryAgreagatorApplication.Main.DAL;
using DeliveryAgreagatorBackendApplication.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace DeliveryAgreagatorApplication.Main.BL.Services
{
    public class OrderService : IOrderService
    {
        private readonly int _pageSize = 6;
        private int _pageCount = 6;
        private string _regexp = "";
        private readonly BackendDbContext _context;
        private readonly IRabbitMqService _rabbitMqService;
        public OrderService(BackendDbContext context, IRabbitMqService rabbitMqService)
        {
            _context = context;
            _rabbitMqService = rabbitMqService;
        }

        public async Task CancelOrder(Guid orderId, Guid userId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x=>x.Id==orderId && x.CustomerId==userId);
            if (order == null)
            {
                throw new WrongIdException(WrongIdExceptionSubject.Order, orderId,"on your account");
            }
            if(order.Status!=Status.Created)
            {
                throw new InvalidOperationException($"You cant cancel order with this {order.Status}");
            }
            order.Status = Status.Canceled;
            await _context.SaveChangesAsync();
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

        public async Task PostOrder(OrderPostDTO model, ClaimsPrincipal userPrincipal)
        {
            Guid userId;
            Guid.TryParse(userPrincipal.FindFirst("IdClaim").Value, out userId);
            var guid = Guid.NewGuid();
            var address = model.Address==null ? userPrincipal.FindFirst("Address").Value : model.Address;
            if (address == null)
            {
                throw new InvalidOperationException("You should choose address!");
            }
            var dishesInCart = _context.DishInCart.Include(x=>x.Dish).Where(x => x.CustomerId == userId && x.Active).ToList();
            if (dishesInCart.Count == 0 )
            {
                throw new InvalidOperationException("Your cart is empty!");
            }
            if (dishesInCart.Any(x => x.Dish.RestaurantId != dishesInCart[0].Dish.RestaurantId)){
                throw new InvalidOperationException("You can't order dishes from different restaurants at the same time!"); 
            }
            var order = new OrderDbModel
            {
                Id = guid,
                RestaurantId= dishesInCart[0].Dish.RestaurantId,
                CustomerId = userId,
                DishesInCart = dishesInCart,
                Number = guid.GetHashCode(),     
                OrderTime = DateTime.UtcNow,
                Address = address,
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
                throw new WrongIdException(WrongIdExceptionSubject.Order, orderId, "on your account");
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

        public async Task<List<OrderDTO>> GetOrdersAvaliableToCook(bool active, DateSort? sort, int page, Guid cookId)
        {
            var cook = await _context.Cooks.FindAsync(cookId);
            var orders = _context.Orders.Include(x => x.DishesInCart).ThenInclude(x => x.Dish).Where(c => c.RestaurantId == cook.RestaurantId && ( active ? (c.Status==Status.Created || (c.Status<Status.Packed && c.CookId==cookId)) : (c.Status>=Status.Packed && c.CookId == cookId))).ToList(); 

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

        public async Task TakeOrderCook(Guid orderId, Guid cookId, StatusDTO status)
        {
            if (status.Status==Status.Kitchen)
            {
                var cook = await _context.Cooks.FindAsync(cookId);
                var order = await _context.Orders.Include(x => x.DishesInCart).ThenInclude(x => x.Dish).FirstOrDefaultAsync(c => c.Id == orderId && c.RestaurantId == cook.RestaurantId);
                if (order == null)
                {
                    throw new InvalidOperationException($"You cannot take order with this ${orderId} id!");
                }
                if (order.Status != Status.Created)
                {
                    throw new InvalidOperationException($"You cannot set this {status.Status} Status to this {orderId} order!");
                }
                order.Status = status.Status;
                order.CookId = cookId;
                await _rabbitMqService.SendMessage(new Notification { OrderId = order.Id, Status = 0, UserId = order.CustomerId, Text ="time" }); //TODO: нормальный текст
            }
            else if(status.Status==Status.Packaging || status.Status==Status.Packed)
            {
                var order = await _context.Orders.FirstOrDefaultAsync(x=>x.Id== orderId && x.CookId==cookId && (x.Status==Status.Kitchen || x.Status == Status.Packaging));
                if (order == null)
                {
                    throw new InvalidOperationException($"You haven't got order in progress with this ${orderId} id!");
                }
                if (order.Status == status.Status-1)
                {
                    order.Status = status.Status;
                }
                else
                {
                    throw new InvalidOperationException($"You cannot set this {status.Status} Status to this {orderId} order!");
                }
                await _rabbitMqService.SendMessage(new Notification { OrderId = order.Id, Status = 0, UserId = order.CustomerId, Text="time" }); //TODO: нормальный текст
            }
            else
            {
                throw new InvalidOperationException($"You cannot set this {status.Status} Status to this {orderId} order!");
            }
            await _context.SaveChangesAsync();

        }


        public async Task<List<OrderDTO>> GetOrdersAvaliableToCourier(Guid coourierId)
        {
            var orders = _context.Orders.Where(x => x.Status == Status.Packed);
            var ordersDTO = orders.Select(x=>x.ConvertToDTO()).ToList() ;
            return ordersDTO;
        }

        public async Task TakeOrderCourier(Guid orderId, Guid courierId, StatusDTO status)
        {
            if (status.Status == Status.Delivery)
            {
                var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId && x.Status == Status.Packed);
                if (order == null)
                {
                    throw new InvalidOperationException($"You can't take order with this {orderId} id!"); //TODO: сделать более точные исключения
                }
                if (order.Status != Status.Packed)
                {
                    throw new InvalidOperationException($"You cannot set this {status.Status} Status to this {orderId} order!");
                }
                order.DeliveryTime = DateTime.UtcNow.AddHours(1);
                order.Status = status.Status;
                order.CourierId = courierId;
                await _rabbitMqService.SendMessage(new Notification { OrderId = order.Id, Status = 0, UserId = order.CustomerId, Text = "time" }); //TODO: нормальный текст
            }
            else if (status.Status == Status.Delivered)
            {
                var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId && x.Status == Status.Delivery && x.CourierId==courierId);
                if (order == null)
                {
                    throw new InvalidOperationException($"You can't modify order with this {orderId} id!"); //TODO: сделать более точные исключения
                }
                order.Status = status.Status;
                await _rabbitMqService.SendMessage(new Notification { OrderId = order.Id, Status = 0, UserId = order.CustomerId, Text = "time" }); //TODO: нормальный текст
            }
            else
            {
                throw new InvalidOperationException($"You cannot set this {status.Status} Status to this {orderId} order!");
            }
            await _context.SaveChangesAsync(); 
        }

        public async Task CancelOrderCourier(Guid orderId, Guid courierId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId && x.CourierId == courierId);
            if (order == null)
            {
                throw new WrongIdException(WrongIdExceptionSubject.Order,orderId, "in proccess");
            }
            if (order.Status != Status.Delivery)
            {
                throw new InvalidOperationException($"You cant cancel order with this {order.Status.ToString()}");
            }
            order.Status = Status.Canceled;
            await _rabbitMqService.SendMessage(new Notification { OrderId = order.Id, Status = 0, UserId = order.CustomerId, Text = "time" }); //TODO: нормальный текст
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
