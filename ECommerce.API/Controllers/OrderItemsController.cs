using Microsoft.AspNetCore.Mvc;
using ECommerce.API.Context;
using ECommerce.API.Entities;
using ECommerce.API.DTOs.OrderItemDtos;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly ECommerceContext _context;
        public OrderItemsController(ECommerceContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetOrderItemsList()
        {
            var values=_context.OrderItems.ToList();
            return Ok(values);
        }

        [HttpPost]
        public IActionResult CreateOrderItem(CreateOrderItemDto dto)
        {
            OrderItem newItem = new OrderItem
            {
                OrderId = dto.OrderId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice
            };
            _context.OrderItems.Add(newItem);
            _context.SaveChanges();
            return Ok("Sipariş Kalemi Başarıyla Eklendi!");
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteOrderItem(int id)
        {
            var value = _context.OrderItems.Find(id);
            if (value == null)
            {
                return NotFound("Silinecek Sipariş Kalemi Bulunamadı!"); 
            }
            _context.OrderItems.Remove(value);
            _context.SaveChanges();
            return Ok("Sipariş Kalemi Başarıyla Silindi!");
        }


        [HttpPut]
        public IActionResult UpdateOrdeItem(UpdateOrderItemDto dto)
        {
            var value = _context.OrderItems.Find(dto.Id);
            if (value == null)
            {
                return NotFound("Güncellenecek Sipariş Kalemi Bulunamadı!");
            }
            value.OrderId = dto.OrderId;
            value.ProductId = dto.ProductId;
            value.Quantity = dto.Quantity;
            value.UnitPrice = dto.UnitPrice;

            _context.SaveChanges();
            return Ok("Sipariş Kalemi Başarıyla GÜncellendi!");
        }

    }
}
