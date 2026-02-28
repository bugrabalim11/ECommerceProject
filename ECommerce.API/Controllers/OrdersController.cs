using Microsoft.AspNetCore.Mvc;
using ECommerce.API.Context;
using ECommerce.API.Entities;
using ECommerce.API.DTOs.OrderDtos;
using Microsoft.EntityFrameworkCore;

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
           var values = _context.Orders
                .Include(x => x.User)  // 1. Siparişi veren Müşteriyi (User) dahil et
                .Include(x=> x.OrderItems)  // 2. Siparişin içindeki sepet detaylarını (OrderItems) dahil et
                .ThenInclude(y=> y.Product)  // 3. O detayların İÇİNE GİR ve asıl Ürünü (Product) dahil et!
                .Select(z=> new ResultOrderDto  // İŞTE SİHİR BURADA BAŞLIYOR!
                {
                    OrderId = z.OrderId,
                    CustomerFullName = z.User.UserName + " " + z.User.UserSurname,  // Ad ve Soyadı birleştirdik
                    OrderDate = z.OrderDate,
                    OrderTotalAmount = z.OrderTotalAmount,
                    OrderStatus = z.OrderStatus,


                    // Siparişin içindeki kalemleri de kendi DTO'suna çeviriyoruz:
                    Items = z.OrderItems.Select(w=>new ResultOrderItemDto
                    {
                        ProductName = w.Product.ProductName,  
                        Quantity = w.Quantity,
                        UnitPrice = w.UnitPrice
                    }).ToList()  
                }).ToList();
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
