using brunchie_backend.Models;


using brunchie_backend.DataBase;
using Microsoft.EntityFrameworkCore;


namespace brunchie_backend.Repositories
{
    public class OrderRepository : IOrderRepository
    {

        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task <OrderResponseDto> PlaceOrder(OrderDto orderDto)
        {
            try
            {
              
                Order order = new Order
                {
                    StudentId = orderDto.StudentId,
                    VendorId = orderDto.VendorId,
                    TotalAmount = orderDto.TotalAmount,
                    OrderStatus = "Pending",
                    CreatedAt = DateTime.Now,
                };

              
                await _context.Order.AddAsync(order);
                await _context.SaveChangesAsync(); 

               
                List<OrderItem> orderItems = new List<OrderItem>();

                foreach (var itemDto in orderDto.OrderItems)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.OrderId, 
                        MenuItemId = itemDto.MenuItemId,
                        Quantity = itemDto.Quantity,
                        PriceAtOrder = itemDto.PriceAtOrder
                    };

                    orderItems.Add(orderItem);
                }

                
                await _context.OrderItem.AddRangeAsync(orderItems);
                await _context.SaveChangesAsync();

                return new OrderResponseDto
                {
                    OrderId = order.OrderId,
                    CreatedAt = order.CreatedAt
                };
            }
            catch (DbUpdateException dbEx)
            {
                throw new ApplicationException("An error occurred while updating the database. Please check the data and try again.", dbEx);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred while adding the order.", ex);
            }
        }

        public async Task<IEnumerable<Order>> GetAllStudentOrders(string StudentId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(e => e.Id == StudentId);
            if (user == null)
            {
                throw new KeyNotFoundException("No User Found");
            }

            IEnumerable<Order> orders = await _context.Order
                .Where(e => e.StudentId == StudentId)
                .ToListAsync();

            return orders;


        }

        public async Task<IEnumerable<Order>> GetIncomingOrders(string VendorId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(e => e.Id == VendorId);
            if (user == null)
            {
                throw new KeyNotFoundException("No User Found");
            }


            IEnumerable<Order> orders = await _context.Order
                .Where(o => o.VendorId == VendorId && o.OrderStatus=="Pending" )
                .ToListAsync();

            return orders;

        }

        public async Task UpdateOrderStatus(int OrderId, string VendorId,string status)
        {
            var order = await _context.Order
                        .Where(e => e.VendorId == VendorId && e.OrderId == OrderId)
                        .SingleOrDefaultAsync();

            if (order == null)
            {
                throw new KeyNotFoundException("No User Found");
            }

            order.OrderStatus = status;

            try
            {
                _context.Order.Update(order);

                await _context.SaveChangesAsync();

                
            }

            catch (DbUpdateException dbEx)
            {
                throw new ApplicationException("An error occurred while updating the database. Please check the data and try again.", dbEx);
            }

            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred while adding the order.", ex);
            }

        }

        public async Task<IEnumerable<OrderItemResponseDto>> GetOrderItems(int OrderId)
        {
            var items = await (from orderItem in _context.OrderItem
                              join menu in _context.MenuItem
                              on orderItem.MenuItemId equals menu.ItemId
                              where orderItem.MenuItemId == menu.ItemId
                              select new OrderItemResponseDto
                              {
                                
                                  MenuItemName = menu.Name,
                                  MenuItemDescription = menu.Description,
                                  Quantity = orderItem.Quantity,
                                  PriceAtOrder = orderItem.PriceAtOrder
                              }).ToListAsync();

            return items;



        }

    }
}
 