using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ECommerce.MVC.DTOs.ProductDtos;
using System.Text;
using System.Net.Http.Headers;
using ECommerce.MVC.DTOs.CategoryDtos;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public async Task<IActionResult> CreateProduct()
        {
            // --- SENİN HARİKA TESPİTİN: GÜVENLİK DUVARI ---
            var token = Request.Cookies["ECommerceJwt"];
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Index", "Login");
            // ----------------------------------------------

            var client = _httpClientFactory.CreateClient();

            // 1. Ürün ekleme sayfasını açmadan önce, API'den Kategori listesini çekiyoruz
            var responseMessage = await client.GetAsync("https://localhost:7107/api/Categories");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonSerializer.Deserialize<List<ResultCategoryDto>>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // 2. Gelen veriyi HTML'deki Dropdown'un (Açılır Liste) anlayacağı dile (SelectListItem) çeviriyoruz
                List<SelectListItem> categoryValues = (from x in values
                                                       select new SelectListItem
                                                       {
                                                           Text = x.CategoryName,   // Ekranda görünecek yazı (Örn: Telefon)
                                                           Value = x.CategoryId.ToString()  // Arka planda seçilecek ID (Örn: 1)
                                                       }).ToList();

                // 3. Bu listeyi ViewBag adlı sihirli çantaya koyup View'a gönderiyoruz
                ViewBag.CategoryList = categoryValues;
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
            // Güvenlik Duvarı
            var token = Request.Cookies["ECommerceJwt"];
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Index", "Login");

            var client = _httpClientFactory.CreateClient();

            // --- 1. EKLENEN KISIM: KATEGORİ LİSTESİNİ ÇEK ---
            var categoryResponse = await client.GetAsync("https://localhost:7107/api/Categories");
            if (categoryResponse.IsSuccessStatusCode)
            {
                var categoryJson = await categoryResponse.Content.ReadAsStringAsync();
                var categories = JsonSerializer.Deserialize<List<ResultCategoryDto>>(categoryJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                List<SelectListItem> categoryValues = (from x in categories
                                                       select new SelectListItem
                                                       {
                                                           Text = x.CategoryName,
                                                           Value = x.CategoryId.ToString()
                                                       }).ToList();
                ViewBag.CategoryList = categoryValues;
            }
            // ------------------------------------------------

            // 2. GÜNCELLENECEK ÜRÜNÜN BİLGİLERİ (Zaten Vardı)
            var responseMessage = await client.GetAsync($"https://localhost:7107/api/Products/{id}");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonSerializer.Deserialize<UpdateProductDto>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

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