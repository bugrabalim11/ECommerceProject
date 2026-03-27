# 🛒 Benim Mağazam - E-Ticaret Platformu

Bu proje, modern web geliştirme standartlarına uygun olarak tasarlanmış, **N-Katmanlı (N-Tier) Mimari** üzerine inşa edilmiş tam kapsamlı bir B2C e-ticaret uygulamasıdır. 

## 📸 Ekran Görüntüleri

![Ana Sayfa](https://github.com/user-attachments/assets/b5ad0d8e-040e-41bf-9c79-0cf3c0412ebc)

![Ürünler Sayfası](https://github.com/user-attachments/assets/63c44fbe-9db0-4c4c-8912-174bd47fd62a)

![Kategoriler Sayfası](https://github.com/user-attachments/assets/cbdcedb0-dd79-4cd3-82e0-75df250891c5)

![Siparişler Sayfası](https://github.com/user-attachments/assets/aaea336e-c38c-4570-aeff-4df794d2d4f9)

![Kullanıcılar Sayfası](https://github.com/user-attachments/assets/c0876fc0-7aab-4b84-a807-5dc66fcc66a6)

![Sepet Sayfası](https://github.com/user-attachments/assets/a3931769-bdb3-43d4-9b0a-7554ff602108)

![Profil Sayfası](https://github.com/user-attachments/assets/e19c88b6-4ca4-4ac3-bfe6-ac66097568b5)

## 🚀 Öne Çıkan Özellikler

* **Güvenli Kimlik Doğrulama:** JWT (JSON Web Token) tabanlı güvenli giriş ve kayıt sistemi.
* **API & MVC Haberleşmesi:** Arka plan işlemleri bağımsız bir ASP.NET Core Web API üzerinden yürütülürken, kullanıcı arayüzü .NET Core MVC mimarisi ile sunulmaktadır.
* **Dinamik Sepet Yönetimi:** Anlık toplam tutar hesaplamalarının yapıldığı, sepet içi miktar güncellemelerini destekleyen interaktif altyapı.
* **Güvenli Profil Yönetimi:** Kullanıcı verilerinin Token üzerinden okunarak, form manipülasyonlarına karşı korunduğu ve yetkisiz erişimlerin engellendiği güvenli mimari.
* **Kullanıcı Deneyimi (UX):** SweetAlert2 ve Bootstrap 5 kullanılarak tasarlanmış modern, duyarlı (responsive) ve anlık geri bildirim veren şık arayüz.

## 🛠️ Kullanılan Teknolojiler

* **Backend:** C#, ASP.NET Core Web API, .NET Core MVC
* **Veritabanı:** Microsoft SQL Server, Entity Framework Core / SQL Scripting
* **Mimari & Araçlar:** N-Tier Architecture, AutoMapper, JWT Bearer Authentication, Dependency Injection
* **Frontend:** HTML5, CSS3, Bootstrap 5, SweetAlert2, FontAwesome

## 🔑 Test Hesapları (Admin Girişi)

Projeyi yerel ortamınızda test ederken tüm yetkilere (Admin) sahip olmak için aşağıdaki giriş bilgilerini kullanabilirsiniz:

* **E-Posta:** `bugra@gmail.com`
* **Şifre:** `123`

## ⚙️ Kurulum ve Çalıştırma

1. **Projeyi Klonlayın:**
   ```bash
  git clone [https://github.com/bugrabalim11/ECommerceProject.git](https://github.com/bugrabalim11/ECommerceProject.git)

2. Veritabanını Hazırlayın:

Proje ana dizininde bulunan ECommerce_DB.sql dosyasını SQL Server Management Studio (SSMS) üzerinden açıp çalıştırarak veritabanı tablolarını anında oluşturabilirsiniz.

3. Bağlantı Ayarları:

API projesi içerisindeki appsettings.json dosyasında bulunan SQL bağlantı dizesini kendi yerel sunucunuza göre güncelleyin.

4. Çalıştırma:

Visual Studio üzerinden Solution'a sağ tıklayıp "Set Startup Projects" diyerek "Multiple startup projects" ayarını seçin.

Hem API hem de MVC projelerinin eylemini "Start" olarak ayarlayıp projeyi ayağa kaldırın.

👨‍💻 Geliştirici Notları ve Mentörlük
Bu proje, "Tutorial Hell" (eğitim cehennemi) tuzağına düşmeden, teorik bilgileri doğrudan pratiğe dökmek amacıyla geliştirilmiştir.

Projenin geliştirme sürecinde sadece kod yazmakla kalınmamış; API güvenliği, DTO (Data Transfer Object) mimarisi ve N-Tier yapısı derinlemesine incelenmiştir. Kodlama sürecinde kas hafızasını geliştirmek için kodlar kopyalanmadan bizzat yazılmış ve yapay zeka (Google Gemini) bir "Pair Programmer & Senior Mentor" olarak kullanılarak hata ayıklama (debug), jargon çevirmenliği ve sektör standartları (Best Practices) konularında destek alınmıştır.
