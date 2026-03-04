using ECommerce.API.Interfaces;
using ECommerce.API.Entities;
using ECommerce.API.DTOs.OrderItemDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Kapıda güvenlik var
    public class OrderItemsController : ControllerBase
    {
        // Artık veritabanıyla (Context) değil, servisimizle (Aşçı) konuşuyoruz
        private readonly IOrderItemService _orderItemService;

        public OrderItemsController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderItemsList()
        {
            var values = await _orderItemService.GetAllOrderItemsAsync();
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderItem(CreateOrderItemDto dto)
        {
            // DTO'yu Entity'ye çevirip servise gönderiyoruz
            var newItem = new OrderItem
            {
                OrderId = dto.OrderId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice
            };

            await _orderItemService.AddOrderItemAsync(newItem);
            return Ok("Sipariş Kalemi Başarıyla Eklendi!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            await _orderItemService.DeleteOrderItemAsync(id);
            return Ok("Sipariş Kalemi Başarıyla Silindi!");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrderItem(UpdateOrderItemDto dto)
        {
            // Önce var olup olmadığını servisten kontrol edebiliriz
            var existingItem = await _orderItemService.GetOrderItemByIdAsync(dto.Id);
            if (existingItem == null)
            {
                return NotFound("Güncellenecek Sipariş Kalemi Bulunamadı!");
            }

            existingItem.OrderId = dto.OrderId;
            existingItem.ProductId = dto.ProductId;
            existingItem.Quantity = dto.Quantity;
            existingItem.UnitPrice = dto.UnitPrice;

            await _orderItemService.UpdateOrderItemAsync(existingItem);
            return Ok("Sipariş Kalemi Başarıyla Güncellendi!");
        }
    }
}
