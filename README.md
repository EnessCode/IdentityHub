# IdentityHub

**IdentityHub**, ASP.NET Core 9 MVC mimarisi kullanılarak geliştirilmiş, gelişmiş kimlik doğrulama, rol tabanlı erişim kontrolü (RBAC) ve dış sistemler için JWT destekli API uç noktaları sunan kapsamlı bir yönetim paneli ve kimlik yönetim projesidir.

Proje, modern web güvenliği standartlarına uygun olarak tasarlanmış olup; web tarayıcıları için geleneksel **Cookie (Çerez) tabanlı** yetkilendirmeyi ve mobil/harici istemciler için **JWT (JSON Web Token)** doğrulamasını aynı sistem üzerinde hibrit (çift yönlü) bir mimari ile birleştirir.

## 🚀 Temel Özellikler

* **Gelişmiş Kimlik Yönetimi (ASP.NET Core Identity):** 
  * Kullanıcı kayıt, giriş, profil düzenleme işlemleri.
  * SMTP üzerinden güvenli e-posta onayı ve "Şifremi Unuttum" (şifre sıfırlama) akışları.
* **Sosyal Medya Entegrasyonu:**
  * Güvenlik açıklarına (Account Enumeration) karşı korumalı ve e-posta çakışmalarını önleyen **Google OAuth 2.0** ile tek tıkla giriş desteği.
* **Çift Yönlü Kimlik Doğrulama Mimarisi (Dual Auth):**
  * MVC sayfaları ve yönetim paneli için güvenli **Cookie tabanlı** oturum yönetimi.
  * Dış API tüketicileri (Mobil uygulamalar, SPA'ler) için izole edilmiş **JWT tabanlı** API uç noktaları (`ApiAuthController`).
* **Gelişmiş Yönetim Paneli:**
  * **Kullanıcı Yönetimi:** Kullanıcıları listeleme, dinamik rol atama ve hesapları tek tıkla **Aktif/Askıya Alınmış (Pasif)** duruma getirme.
  * **Kategori Yönetimi:** İkon destekli ve aktiflik/pasiflik durumu barındıran kategoriler için tam CRUD (Create, Read, Update, Delete) operasyonları.
* **Akıllı Güvenlik ve Moderasyon:**
  * Aktif olmayan kullanıcıların (hem standart giriş hem de Google girişi üzerinden) sisteme sızmasını engelleyen katı güvenlik duvarları.
  * Yorumlar ve forum yapıları için ML.NET destekli çevrimdışı metin moderasyonu altyapısı.
* **Özel Hata Yönetimi:** Yetkisiz erişimler ve bulunamayan sayfalar için özelleştirilmiş 401, 403 ve 404 sayfaları.

## 🛠️ Kullanılan Teknolojiler

* **Backend:** C#, .NET 9, ASP.NET Core MVC, ASP.NET Core Web API
* **Veritabanı & ORM:** MS SQL Server, Entity Framework Core
* **Güvenlik & Kimlik Doğrulama:** ASP.NET Core Identity, JWT (JSON Web Tokens), Google.Apis.Auth
* **Arayüz (Frontend):** HTML5, CSS3, Bootstrap 4/5, jQuery

## 📂 Mimari Yapı ve Tasarım Desenleri

* **MVC (Model-View-Controller):** Kullanıcı arayüzü ve iş mantığının birbirinden temiz bir şekilde ayrılması.
* **Dependency Injection (Bağımlılık Enjeksiyonu):** `ITokenService`, `IEmailService` gibi servislerin gevşek bağlı (loosely coupled) bir mimaride sisteme dahil edilmesi.
* **Asenkron Programlama:** `async/await` yapıları ile veritabanı sorgularında ve harici servis çağrılarında (Email, Google Auth) yüksek performans.
