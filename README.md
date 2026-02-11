# 🏧 ATM Recipe App

ATM (Automated Teller Machine) ürünlerinin reçetelerini yöneten web uygulaması. Ürünlerin hangi bileşenlerden oluştuğunu ve toplam maliyetini hesaplayan bir yönetim sistemi.

## 📋 Özellikler

- ✅ **Ürün Yönetimi**: ATM ürünlerini ekle, düzenle, sil
- ✅ **Bileşen Yönetimi**: Bileşenleri (LCD, CPU vb.) ve fiyatlarını yönet
- ✅ **Reçete Yönetimi**: Ürünlere bileşen ata, miktarlarını belirle
- ✅ **Maliyet Hesaplama**: Toplam maliyeti otomatik hesapla
- ✅ **Modern UI**: Bootstrap ile responsive tasarım

## 🛠️ Teknolojiler

- **Backend**: ASP.NET Core MVC 10.0
- **Database**: Microsoft SQL Server
- **ORM**: Entity Framework Core
- **Frontend**: Razor Views, Bootstrap 5
- **Language**: C# 13

## 📦 Kurulum

### Gereksinimler
- .NET 10.0 SDK ([İndir](https://dotnet.microsoft.com/download))
- Docker Desktop ([İndir](https://www.docker.com/products/docker-desktop))
- Azure Data Studio (Opsiyonel - test verileri için)

### Kurulum Adımları

#### 1️⃣ **Projeyi Klonlayın**
```bash
git clone https://github.com/KoracM/AtmRecipe.git
cd AtmRecipe
```

#### 2️⃣ **SQL Server'ı Docker'da Başlatın**

**⚠️ ÖNEMLİ:** Kendi şifrenizi belirleyin! (`YourStrongPassword123!` yerine güçlü bir şifre kullanın)

```bash
# macOS / Linux
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrongPassword123!" \
  -p 1433:1433 --name mssql-atm -d mcr.microsoft.com/mssql/server:2022-latest

# Windows PowerShell
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrongPassword123!" -p 1433:1433 --name mssql-atm -d mcr.microsoft.com/mssql/server:2022-latest
```

**Container'ın çalıştığını kontrol edin:**
```bash
docker ps
# mssql-atm görünüyor olmalı
```

#### 3️⃣ **Connection String'i Güncelleyin** ⚠️ ZORUNLU

`appsettings.json` dosyasını açın ve `YOUR_PASSWORD_HERE` kısmını Docker'da belirlediğiniz şifreyle değiştirin:

**Değiştirmeden önce:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=AtmRecipeDB;User ID=SA;Password=YOUR_PASSWORD_HERE;..."
  }
}
```

**Değiştirdikten sonra:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=AtmRecipeDB;User ID=SA;Password=YourStrongPassword123!;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

#### 4️⃣ **Bağımlılıkları Yükleyin**
```bash
dotnet restore
```

#### 5️⃣ **Uygulamayı Çalıştırın**
```bash
dotnet watch run
```

İlk çalıştırmada `AtmRecipeDB` database'i otomatik oluşturulur.

#### 6️⃣ **(Opsiyonel) Test Verilerini Yükleyin**

Azure Data Studio veya SQL Server Management Studio ile:
- **Server**: `localhost,1433`
- **Authentication**: SQL Login
- **Username**: `SA`
- **Password**: Docker'da belirlediğiniz şifre

Bağlandıktan sonra `TestData1.sql` dosyasını çalıştırın.

#### 7️⃣ **Tarayıcıda Açın**
```
http://localhost:5217
```

---

## 📖 Kullanım

### 1. Bileşen Ekle
- Navbar → 🔧 Bileşen Yönetimi → Yeni Bileşen Ekle
- Örnek: "LCD Screen", Fiyat: 500₺

### 2. Ürün Oluştur
- Navbar → 📦 Ürün Yönetimi → Yeni Ürün Ekle
- Örnek: "ATM Super"

### 3. Bileşen Ata
- Ürün Listesi → 🔧 Bileşen Ekle ve Düzenle
- Dropdown'dan bileşen seç, miktar gir, kaydet

### 4. Reçete Görüntüle
- Navbar → 🏧 ATM Reçete
- Ürün seç → Bileşenler ve toplam maliyet görüntülenir

---

## 📁 Proje Yapısı

```
AtmRecipeApp/
├── Controllers/          # MVC Controllers
│   ├── ProductsController.cs
│   ├── ComponentsController.cs
│   └── RecipeController.cs
├── Models/              # Entity models
│   ├── Product.cs
│   ├── Component.cs
│   ├── ProductComponent.cs
│   └── AtmRecipeContext.cs
├── Views/               # Razor views
│   ├── Products/
│   ├── Components/
│   └── Recipe/
└── TestData1.sql        # Sample data
```

---

## 🔧 Development

### Hot Reload
```bash
dotnet watch run
```
View ve Controller değişiklikleri otomatik yüklenir.

### Database Yeniden Oluşturma
```bash
# Container'ı yeniden başlat
docker restart mssql-atm

# Veya container'ı tamamen sil ve yeniden oluştur
docker rm -f mssql-atm
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourPassword123!" \
  -p 1433:1433 --name mssql-atm -d mcr.microsoft.com/mssql/server:2022-latest

# Uygulama otomatik database oluşturur
dotnet run
```

---

## 🐛 Bilinen Sorunlar

- **Decimal precision uyarısı**: Logger'da görünür ama çalışmayı etkilemez
- **HTTPS redirect uyarısı**: Development modunda önemsiz

---

## ❓ Sık Sorulan Sorular

### Docker container başlamıyor?
```bash
# Container'ı kontrol et
docker logs mssql-atm

# Port kullanımda mı?
lsof -i :1433  # macOS/Linux
netstat -ano | findstr :1433  # Windows
```

### Database connection hatası?
- `appsettings.json`'daki şifrenin Docker şifresiyle aynı olduğundan emin olun
- Container'ın çalıştığını kontrol edin: `docker ps`

### Hot Reload çalışmıyor?
- Ctrl+C ile durdurun
- `dotnet watch run` ile tekrar başlatın

---

## 📝 Lisans

Bu proje eğitim amaçlıdır.

## 👨‍💻 Geliştirici

ASP.NET MVC öğrenim projesi - Spring Boot'tan .NET'e geçiş
