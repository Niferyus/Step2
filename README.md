# Step2 Projesi

## Genel Bakış

Step2, .NET 8 ile geliştirilmiş, CQRS mimarisi kullanan, JWT tabanlı kimlik doğrulama ve Redis ile önbellekleme özelliklerine sahip bir Web API projesidir. PostgreSQL veritabanı ve Entity Framework Core ile veri yönetimi sağlanır. Proje, ürün ve kullanıcı yönetimi için temel CRUD işlemlerini ve kimlik doğrulama akışlarını içerir.

## Proje Mimarisi

- **Core**: Temel varlıklar (`User`, `Product`) ve modeller.
- **Application**: CQRS komutları, sorguları, DTO'lar ve arayüzler.
- **Infrastructure**: Veritabanı erişimi, JWT, şifreleme, Redis servisleri ve Unit of Work.
- **WebApi**: API uç noktaları, middleware ve uygulama başlangıcı.

## Kullanılan Teknolojiler

- .NET 8
- Entity Framework Core & Npgsql
- MediatR (CQRS için)
- JWT Authentication
- Redis (StackExchange.Redis)
- Serilog (loglama)
- Docker

## Kurulum

1. **Gereksinimler**
   - .NET 8 SDK
   - PostgreSQL (varsayılan bağlantı: `localhost:5432`, veritabanı: `steptwo`)
   - Redis (varsayılan bağlantı: `localhost:6379`)
   - Docker

2. **Veritabanı Migrasyonları**
   ```sh
   dotnet ef database update --project Infrastructure
   ```

3. **Uygulamayı Çalıştırma**
   ```sh
   dotnet run --project WebApi
   ```
   API varsayılan olarak `http://localhost:5292` adresinde çalışır.

4. **Docker ile Çalıştırma**
   ```sh
   docker build -t step2-api -f WebApi/Dockerfile .
   docker run -p 8080:8080 -p 8081:8081 step2-api
   ```

## API Uç Noktaları

- `POST /api/auth/register` : Kullanıcı kaydı
- `POST /api/auth/login` : Kullanıcı girişi (JWT döner)
- `GET /api/product` : Tüm ürünleri getirir
- `GET /api/product/{id}` : Ürün detayını getirir
- `POST /api/product` : Yeni ürün ekler
- `PUT /api/product` : Ürün günceller
- `DELETE /api/product?id={id}` : Ürün siler

## Önbellekleme

- Ürün sorguları Redis ile önbelleğe alınır.
- Ürün ekleme/güncelleme/silme işlemlerinde ilgili cache temizlenir.

## Kimlik Doğrulama

- JWT tabanlı kimlik doğrulama.
- Kayıt ve giriş işlemlerinde token döner.

## Loglama

- Serilog ile konsol ve dosya tabanlı loglama.
- Loglar `WebApi/logs/` klasöründe tutulur.
