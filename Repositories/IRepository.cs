using brunchie_backend.Models;
using Org.BouncyCastle.Security.Certificates;

namespace brunchie_backend.Repositories
{
    public interface IOrderRepository
    {
        Task<OrderResponseDto> PlaceOrder(OrderDto order);

        Task UpdateOrderStatus(int OrderId,string VendorId,string status);

        Task<IEnumerable<Order>> GetAllStudentOrders(string StudentId);


        Task<IEnumerable<Order>> GetIncomingOrders(string VendorId);

        Task<IEnumerable<OrderItemResponseDto>> GetOrderItems(int OrderId);

    }




    public interface IMenuRepository
    {
        Task AddItems(IEnumerable<MenuItemAddDto> items);
        Task<IEnumerable<int>> RemoveItems(IEnumerable<int> items);

        Task <IEnumerable<MenuItemRepDto>> GetMenu(string VendorId);

       
       
    }
}


