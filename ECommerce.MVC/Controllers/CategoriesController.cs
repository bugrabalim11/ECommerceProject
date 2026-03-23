using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ECommerce.MVC.DTOs.CategoryDtos;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization; // 👈 Mutlaka ekle!

namespace ECommerce.MVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CategoriesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // 🟢 Herkes görebilir
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7107/api/Categories");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonSerializer.Deserialize<List<ResultCategoryDto>>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return View(values);
            }
            return View();
        }

        // 🛑 SADECE ADMİN: Kategori Ekleme Sayfası
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateCategory()
        {
            // Sadece boş sayfayı açar (Formu gösterir)
            return View();
        }

        // 🛑 SADECE ADMİN: Kategori Kaydetme
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            // 1. Cüzdana bak, anahtar yoksa Login kapısına yolla!
            var token = Request.Cookies["ECommerceToken"];
            // Artık token kontrolünü [Authorize] yapıyor, direkt işe girişebiliriz
            var client = _httpClientFactory.CreateClient();

            // 2. Anahtarı Garsonun eline ver
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // 3. Çantayı (DTO) hazırla ve Mutfağa (API) fırlat
            var jsonData = JsonSerializer.Serialize(createCategoryDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PostAsync("https://localhost:7107/api/Categories", stringContent);

            return responseMessage.IsSuccessStatusCode ? RedirectToAction("Index") : View();
        }

        // 🛑 SADECE ADMİN: Kategori Silme
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var token = Request.Cookies["ECommerceToken"];
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            await client.DeleteAsync($"https://localhost:7107/api/Categories/{id}");
            return RedirectToAction("Index");
        }

        // 🛑 SADECE ADMİN: Güncelleme Formu
        [Authorize(Roles = "Admin")]
        [HttpGet]  // Tablodaki kategorşleri listeledik
        public async Task<IActionResult> UpdateCategory(int id)
        {
            // 1. Garson mutfağa gidip güncellenecek kategorinin "eski" bilgilerini istiyor
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:7107/api/Categories/{id}");

            if (responseMessage.IsSuccessStatusCode)
            {
                // 2. Gelen JSON verisini UpdateCategoryDto çantasına dolduruyoruz
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonSerializer.Deserialize<UpdateCategoryDto>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // 3. İçi dolu çantayı View'a (Forma) gönderiyoruz
                return View(values);
            }

            return RedirectToAction("Index");
        }

        // 🛑 SADECE ADMİN: Güncelleme Kaydı
        [Authorize(Roles = "Admin")]
        [HttpPost]  // ID ile seçip güncelledik
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
            // 1. Cüzdandaki anahtarı alalım
            var token = Request.Cookies["ECommerceToken"];
           

            var client = _httpClientFactory.CreateClient();

            // 2. Anahtarı Garsonun yakasına tak
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // 3. Yeni verileri çantaya koy ve API'ye fırlat (Dikkat: PostAsync değil, PutAsync kullanıyoruz!)
            var jsonData = JsonSerializer.Serialize(updateCategoryDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PutAsync("https://localhost:7107/api/Categories", stringContent);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index"); // Başarılıysa listeye dön
            }

            return View();
        }
    }
}
