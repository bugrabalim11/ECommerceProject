using ECommerce.API.Interfaces;
using ECommerce.API.Entities;
using ECommerce.API.DTOs.OrderItemDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper; // DTO dönüşümleri için gerekli

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Kapıda güvenlik var
    public class OrderItemsController : ControllerBase
    {
        // Artık veritabanıyla (Context) değil, servisimizle (Aşçı) konuşuyoruz
        private readonly IOrderItemService _orderItemService;
        private readonly IMapper _mapper; // DTO dönüşümleri için

        public OrderItemsController(IOrderItemService orderItemService, IMapper mapper)
        {
            _orderItemService = orderItemService;
            _mapper = mapper;
        }

        // 🛑 KİLİTLİ: Tüm sipariş kalemlerini listelemek Admin'e özeldir.
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetOrderItemsList()
        {
            // ESKİSİ: var values = await _orderItemService.GetAllOrderItemsAsync();
            // YENİSİ: Artık Include olanı çağırıyoruz ki ProductName dolu gelsin!
            var values = await _orderItemService.GetOrderItemsWithProductAsync();

            var mappedValues = _mapper.Map<List<ResultOrderItemDto>>(values);
            return Ok(mappedValues);
        }

        // 🟢 AÇIK: Giriş yapmış her üye sepete/siparişe kalem ekleyebilir.
        [HttpPost]
        public async Task<IActionResult> CreateOrderItem(CreateOrderItemDto dto)
        {
            // YENİ VE PROFESYONEL YÖNTEM:
            // AutoMapper DTO'daki bilgileri alıp şıp diye OrderItem nesnesine kopyalıyor!
            var newItem = _mapper.Map<OrderItem>(dto);
            await _orderItemService.AddOrderItemAsync(newItem);
            return Ok("Sipariş Kalemi Başarıyla Eklendi!");
        }

        // 🛑 KİLİTLİ: Bir siparişin içeriğini silmek sadece Admin yetkisindedir.
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            await _orderItemService.DeleteOrderItemAsync(id);
            return Ok("Sipariş Kalemi Başarıyla Silindi!");
        }


        // 🛑 KİLİTLİ: Sipariş kalemini güncellemek (adet, fiyat vs.) Admin işidir.
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateOrderItem(UpdateOrderItemDto dto)
        {
            // Önce var olup olmadığını servisten kontrol edebiliriz
            var existingItem = await _orderItemService.GetOrderItemByIdAsync(dto.Id);
            if (existingItem == null)
            {
                return NotFound("Güncellenecek Sipariş Kalemi Bulunamadı!");
            }

            // SİHİRBAZ DEVREDE: Var olan nesneyi güncelleme!
            // Eskiden tek tek eşitlediğimiz o 4 satırı tek satıra düşürdük.
            _mapper.Map(dto, existingItem);

            await _orderItemService.UpdateOrderItemAsync(existingItem!);
            return Ok("Sipariş Kalemi Başarıyla Güncellendi!");
        }


        // 🛑 KİLİTLİ: Belirli bir siparişin içeriğini ID ile sorgulamak Admin'e özeldir.
        [Authorize(Roles = "Admin")]
        [HttpGet("GetItemsByOrderId/{orderId}")]
        public async Task<IActionResult> GetItemsByOrderId(int orderId)
        {
            // 1. Aşçı zaten verileri vitrinlik (DTO) formatında hazırlayıp getiriyor.
            var allItems = await _orderItemService.GetOrderItemsWithProductAsync();

            // 2. Sadece bizim siparişe ait olanları filtreliyoruz.
            var filteredItems = allItems.Where(x => x.OrderId == orderId).ToList();

            // 3. SİHİRBAZA GEREK YOK! Veri zaten DTO formatında. Direkt masaya servis et!
            return Ok(filteredItems);
        }
    }
}
