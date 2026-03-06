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
    [Authorize]
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

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var values = await _orderService.GetAllOrdersAsync();
            return Ok(_mapper.Map<List<ResultOrderDto>>(values));
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
        {
            var value = _mapper.Map<Order>(dto);
            await _orderService.AddOrderAsync(value);
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
            var existingOrder = await _orderService.GetOrderByIdAsync(dto.OrderId);
            if (existingOrder != null)
            {
                return NotFound("Güncellenecek sipariş bulunamadı!");
            }

            _mapper.Map(dto, existingOrder);
            await _orderService.UpdateOrderAsync(existingOrder!);
            return Ok("Sipariş Başarıyla Güncellendi!");
        }
    }
}