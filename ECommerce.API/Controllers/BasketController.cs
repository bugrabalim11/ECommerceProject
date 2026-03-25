using AutoMapper;
using ECommerce.API.DTOs.BasketDtos;
using ECommerce.API.Entities;
using ECommerce.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    // 🔒 KİLİTLİ: Dükkana (Sisteme) giriş yapmayan hiç kimse sepet işlemlerine erişemez!
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly IMapper _mapper;

        public BasketController(IBasketService basketService, IMapper mapper)
        {
            _basketService = basketService;
            _mapper = mapper;
        }

        // 🟢 GİRİŞ YAPAN HERKES: Kendi ID'sini gönderip kendi sepetini görebilir
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBasketByUserId(int id)
        {
            // Aşçıdan (Service) saf veriyi al
            var values = await _basketService.GetBasketByUserIdAsync(id);

            // Senin şablonundaki gibi: Garson çantaya (DTO) çevirsin ve müşteriye sunsun
            var MappedValues = _mapper.Map<List<ResultBasketDto>>(values);
            return Ok(MappedValues);
        }

        // 🟢 GİRİŞ YAPAN HERKES: Kendi sepetine yeni ürün ekleyebilir
        [HttpPost]
        public async Task<IActionResult> CreateBasket(CreateBasketDto dto)
        {
            // Gelen çantayı saf veriye (Entity) çevir
            var newBasketItem = _mapper.Map<Basket>(dto);

            await _basketService.AddBasketAsync(newBasketItem);
            return Ok("Ürün sepete başarıyla eklendi!");
        }

        // 🟢 GİRİŞ YAPAN HERKES: Sepetindeki ürünü silebilir (Vazgeçebilir)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasket(int id)
        {
            await _basketService.DeleteBasketAsync(id);
            return Ok("Ürün başarıyla silindi!");
        }

        // 🟢 GİRİŞ YAPAN HERKES: Sepetindeki ürünün adetini güncelleyebilir
        [HttpPut]
        public async Task<IActionResult> UpdateBasket(UpdateBasketDto dto)
        {
            // Çantayı saf veriye (Entity) çevir
            var updatedBasket= _mapper.Map<Basket>(dto);

            // Aşçıya "Bunu güncelle" emrini ver
            await _basketService.UpdateBasketAsync(updatedBasket);
            return Ok("Ürün başarıyla güncellendi!");
        }
    }
}
