using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// 1. Standart Servisler
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient(); // API ile konuşacak garsonumuz

// 2. GÜVENLİK SERVİSİ: Çerez (Cookie) bazlı kimlik doğrulamayı sisteme tanıtıyoruz
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/Login/Index/";        // Giriş yapmamışsa buraya yönlendir
        options.AccessDeniedPath = "/Login/AccessDenied/"; // 👑 Garson yetkisi olmayanları BİZİM tasarladığımız sayfaya atsın
        options.Cookie.Name = "ECommerceCookie";    // Çerezin adı (Opsiyonel)
    });

var app = builder.Build();

// HTTP İstek Hattı Yapılandırması (Sıralama Hayatidir!)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// 3. Statik dosyalar (CSS, JS, Resimler) yetki kontrolüne takılmadan en başta yüklensin
app.MapStaticAssets();

app.UseRouting();

// 🚀 İŞTE O KRİTİK İKİLİ:
app.UseAuthentication(); // ÖNCE: "Bu gelen kim? Elinde geçerli bir çerez var mı?"
app.UseAuthorization();  // SONRA: "Tamam kim olduğunu anladım, peki bu sayfaya girmeye yetkin (Admin rolün) var mı?"

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();