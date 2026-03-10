using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ECommerce.MVC.DTOs.ProductDtos;
using System.Text;
using System.Net.Http.Headers;

namespace ECommerce.MVC.Controllers
{
    public class ProductsController : Controller
    {
        // 1. Garsonu (HttpClientFactory) tanımlıyoruz
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // 2. Sayfa açıldığında çalışacak metot (Index)
        public async Task<IActionResult> Index()
        {
            // Garsonu yarattık
            var client = _httpClientFactory.CreateClient();

            // API adresine gidip ürünleri istiyoruz
            var responseMessage = await client.GetAsync("https://localhost:7107/api/Products");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();

                // JSON'u bizim ResultProductDto çantalarına doldur!
                var values = JsonSerializer.Deserialize<List<ResultProductDto>>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Dolu çantaları Müşteri Salonuna (View'a) gönder!
                return View(values);
            }

            return View();
        }

        // 3. Ürün Ekleme Formunu Ekrana Getiren Metot
        [HttpGet]
        public IActionResult CreateProduct()
        {
            // Cüzdana bakıyoruz
            var token = Request.Cookies["ECommerceJwt"];

            if (string.IsNullOrEmpty(token))
            {
                // Anahtar yoksa Login'e yolla
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        // 4. Form Doldurulup "Kaydet"e Basıldığında Çalışacak Metot
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto createProductDto)
        {
            var token = Request.Cookies["ECommerceJwt"];
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Index", "Login");

            var client = _httpClientFactory.CreateClient();

            // Cüzdandaki anahtarı Garsonun eline veriyoruz!
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var jsonData = JsonSerializer.Serialize(createProductDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PostAsync("https://localhost:7107/api/Products", stringContent);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        // 5. Ürünü Silmek İçin Çalışacak Metot
        [HttpGet]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            // Cüzdandan anahtarı al
            var token = Request.Cookies["ECommerceJwt"];
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Index", "Login");

            var client = _httpClientFactory.CreateClient();

            // Garsona yetki kartını veriyoruz
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // API'ye "Şu numaralı ürünü çöpe at" emri gönderiyoruz (Delete)
            var responseMessage = await client.DeleteAsync($"https://localhost:7107/api/Products/{id}");

            if (responseMessage.IsSuccessStatusCode)
            {
                // Silme tamamsa vitrine geri dön
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        // 6. Güncelleme Formunu (Kutular Dolu Halde) Getiren Metot
        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            var client = _httpClientFactory.CreateClient();

            // Önce API'ye gidip "Bana şu ürünün bilgilerini getir, kutuları dolduracağım" diyoruz
            var responseMessage = await client.GetAsync($"https://localhost:7107/api/Products/{id}");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();

                // Gelen veriyi UpdateProductDto çantasına yerleştiriyoruz
                var value = JsonSerializer.Deserialize<UpdateProductDto>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Dolu çantayı güncelleme sayfasına gönderiyoruz
                return View(value);
            }

            return RedirectToAction("Index");
        }

        // 7. Güncelleme Formu Onaylandığında Çalışacak Metot
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProductDto)
        {
            var token = Request.Cookies["ECommerceJwt"];
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Index", "Login");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Güncellenen verileri paketliyoruz
            var jsonData = JsonSerializer.Serialize(updateProductDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // API'ye "Bu ürünü yeni bilgilerle güncelle" (Put) diyoruz
            var responseMessage = await client.PutAsync("https://localhost:7107/api/Products", stringContent);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}