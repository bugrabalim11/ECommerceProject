using ECommerce.API.Interfaces;
using ECommerce.API.Entities;
using ECommerce.API.DTOs.OrderDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderList()
        {
            // Tüm o Include'lar, Select'ler artık Service'in içinde gizli. 
            // Controller artık çok ferah!
            var values = await _orderService.GetOrdersWithDetailsAsync();
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
        {
            var order = new Order
            {
                UserId = dto.UserId,
                OrderTotalAmount = dto.OrderTotalAmount,
                OrderDate = DateTime.Now,
                OrderStatus = "Sipariş Alındı!"
            };
            await _orderService.AddOrderAsync(order);
            return Ok("Sipariş Başarıyla Oluşturuldu!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return Ok("Sipariş Başarıyla Silindi!");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrder(UpdateOrderDto dto)
        {
            var value = await _orderService.GetOrderByIdAsync(dto.OrderId);
            if (value == null) return NotFound("Güncellenecek Sipariş bulunamadı!");

            value.UserId = dto.UserId;
            value.OrderTotalAmount = dto.OrderTotalAmount;
            value.OrderStatus = dto.OrderStatus;

            await _orderService.UpdateOrderAsync(value);
            return Ok("Sipariş Başarıyla Güncellendi!");
        }
    }
}