using brunchie_backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using brunchie_backend.Models;

namespace brunchie_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderRepository orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        [Authorize(Roles = "Student")]
        [HttpPost("Student")]

        public async Task<IActionResult> Order([FromBody] OrderDto order)

        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var OrderDetails = await orderRepository.PlaceOrder(order);
                return Ok(new { OrderDetails });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        [Authorize(Roles = "Student")]
        [HttpGet("Student")]

        public async Task<IActionResult> ViewStudentOrders([FromQuery] string StudentId)
        {

            if (string.IsNullOrEmpty(StudentId))
            {
                return BadRequest();
            }

            try
            {
                var orders = await orderRepository.GetAllStudentOrders(StudentId);
                return Ok(orders);
            }

            catch (Exception ex)
            {
                return NotFound(new { ex.Message });
            }
        }

        [Authorize(Roles = "Student,Admin")]
        [HttpGet("Items")]

        public async Task<IActionResult> ViewOrderItems([FromQuery] int OrderId)
        {
            

            var OrderItems = await orderRepository.GetOrderItems(OrderId);

            return Ok(OrderItems);
        }
    }

}