using ECommerce.API.Interfaces;
using ECommerce.API.Entities;
using ECommerce.API.DTOs.OrderDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]   // 🔐 Giriş şart!
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;


        // Garson işe başlarken Aşçıyı (Service) ve Sihirbazı (Mapper) yanına alır.
        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }


        // YENİ VE HAVALI METODUMUZ!
        // 🛑 KİLİTLİ: Dükkandaki TÜM siparişleri (detaylı) sadece Admin görebilir.
        [Authorize(Roles = "Admin")]
        [HttpGet("GetOrderWithDetails")]
        public async Task<IActionResult> GetOrdersWithDetails()
        {
            // 1. Aşçıdan (Service) veritabanındaki tüm ham siparişleri detaylarıyla çek.
            var values = await _orderService.GetOrdersWithDetailsAsync();

            // 2. Sihirbaza (AutoMapper) ver, o bize vitrinlik (DTO) listesi versin.
            var mappedValues = _mapper.Map<List<ResultOrderDto>>(values);

            // 3. Masaya (Swagger/İstemci) servis et!
            return Ok(mappedValues);
        }


        // 🛑 KİLİTLİ: Tüm sipariş listesini çekmek Admin'e özeldir.
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var values = await _orderService.GetAllOrdersAsync();
            return Ok(_mapper.Map<List<ResultOrderDto>>(values));
        }


        // 🛑 KİLİTLİ: Bir siparişin detayına ID ile bakmak şimdilik sadece Admin işidir.
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var value = await _orderService.GetOrderByIdAsync(id);

            // SİHİRLİ DOKUNUŞ: Ham nesneyi doğrudan dönmek yerine UpdateOrderDto'ya çeviriyoruz!
            // Böylece ilişkili tabloların yarattığı sonsuz döngüden kurtuluyoruz.
            var mappedValue = _mapper.Map<UpdateOrderDto>(value);

            return Ok(mappedValue);
        }


        // 🟢 AÇIK: Herkes (Giriş yapan) sipariş oluşturabilir.
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
        {
            // 1. DTO'dan gelen verileri (müşteri adı, tutar vs.) Order nesnesine kopyala
            var value = _mapper.Map<Order>(dto);

            // 2. SİHİRLİ DOKUNUŞ: Siparişin oluşturulduğu o anki tarihi (sunucu saatini) bas!
            value.OrderDate = DateTime.Now;

            // 3. Veritabanına kaydet
            await _orderService.AddOrderAsync(value);

            return Ok("Sipariş Başarıyla Oluşturuldu!");
        }


        // 🛑 KİLİTLİ: Sipariş silme yetkisi sadece Admin'dedir.
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return Ok("Sipariş Başarıyla Silindi!");
        }


        // 🛑 KİLİTLİ: Sipariş durumunu güncelleme (Kargoya verildi vs.) Admin işidir.
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateOrder(UpdateOrderDto dto)
        {
            // DİKKAT: Senin kodunda burası != null şeklindeydi. 
            // Yani "Eğer sipariş veritabanında VARSA, bulunamadı hatası ver" diyordun.
            // Doğrusu "Eğer sipariş YOKSA (== null) hata ver" olmalıdır.
            var existingOrder = await _orderService.GetOrderByIdAsync(dto.OrderId);
            if (existingOrder == null)
            {
                return NotFound("Güncellenecek sipariş bulunamadı!");
            }

            _mapper.Map(dto, existingOrder);
            await _orderService.UpdateOrderAsync(existingOrder!);
            return Ok("Sipariş Başarıyla Güncellendi!");
        }
    }
}