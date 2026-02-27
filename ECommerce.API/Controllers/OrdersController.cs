using Microsoft.AspNetCore.Mvc;
using ECommerce.API.Context;
using ECommerce.API.Entities;
using ECommerce.API.DTOs.OrderDtos;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ECommerceContext _context;
        public OrdersController(ECommerceContext context)
        {
            _context = context;
        }

        [HttpGet] 
        public IActionResult GetOrderList()
        {
           var values = _context.Orders.ToList();
            return Ok(values);
        }

        [HttpPost]
        public IActionResult CreateOrder(CreateOrderDto dto)
        {
            Order order = new Order
            {
                UserId = dto.UserId,
                OrderTotalAmount = dto.OrderTotalAmount,
                OrderDate = DateTime.Now,   // Tarihi otomatik verdik!
                OrderStatus = "Sipariş Alındı!"   // İlk sipariş durumunu otomatik atadık!
            };
            _context.Orders.Add(order);
            _context.SaveChanges();
            return Ok("Sipariş Başarıyla Oluşturuldu!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var value = _context.Orders.Find(id);
            if (value == null)
            {
                return NotFound("Silincek Sipariş bulunamadı!");
            }
            _context.Orders.Remove(value);
            _context.SaveChanges();
            return Ok("Sipariş Başarıyla Silindi!");
        }

        [HttpPut]
        public IActionResult UpdateOrder(UpdateOrderDto dto)
        {
            var value = _context.Orders.Find(dto.OrderId);
            if (value == null)
            {
                return NotFound("Güncellenecek Sipariş bulunamadı!");
            }
            value.UserId = dto.UserId;
            value.OrderTotalAmount = dto.OrderTotalAmount;
            value.OrderStatus = dto.OrderStatus;  // Durumu güncelledik (örn: "Kargoya Verildi")
            // OrderDate'i güncellemiyoruz, sabit kalıyor.

            _context.Orders.Update(value);
            _context.SaveChanges();
            return Ok("Sipariş Başarıyla Güncellendi!");
        }
    }
}
