![.NET](https://img.shields.io/badge/.NET-8.0-blueviolet)
![C#](https://img.shields.io/badge/C%23-Programming-green)
![Database](https://img.shields.io/badge/Database-SQLServer-red)

# FaultReportingAPI 🛠️

📌 Proje Özeti

Bu proje, arıza bildirim süreçlerini yönetmek amacıyla geliştirilmiş, modern .NET tabanlı bir Web API uygulamasıdır. Uygulama,  **Repository** ve **Unit of Work**  mimari desenleri üzerine inşa edilerek veri erişim katmanı soyutlanmış, iş mantığının sürdürülebilir ve test edilebilir olması sağlanmıştır.

Sistem, kullanıcıların arıza bildirimleri oluşturmasını, mevcut bildirimlerin durumlarını yönetmesini ve belirli iş kurallarına göre bu durumların güncellenmesini sağlayan bir durum makinesi (state machine) mantığı ile çalışmaktadır. Böylece her arıza kaydı, tanımlı durum geçişlerine uygun şekilde ilerlemektedir.

API geliştirilirken katmanlı mimari prensipleri uygulanmış; **Controller**, **Service**, **Repository** ve **Data Access** katmanları birbirinden ayrıştırılmıştır. Bu sayede kodun okunabilirliği, genişletilebilirliği ve bakım kolaylığı artırılmıştır.

Proje kapsamında ayrıca:

Merkezi hata yönetimi (global exception handling)
Standart response wrapper yapısı
Swagger/OpenAPI dokümantasyonu
Business rule kontrolleri
Enum tabanlı durum yönetimi
gibi API standartlarına uygun yaklaşımlar benimsenmiştir.

Bonus gereksinimler kapsamında:

Belirli bir servis için unit test yazılmıştır (xUnit/NUnit kullanımı)
**Rate limiting** uygulanarak aynı IP’den dakikada maksimum **10** istek sınırı getirilmiştir

Tüm bu yapılar, hem gerçek dünya senaryolarına uygun bir backend mimarisi oluşturmak hem de temiz, ölçeklenebilir ve test edilebilir bir API tasarımı sergilemek amacıyla kurgulanmıştır.
---

## 🚀 Projeyi Çalıştırma Adımları

### 1. 📋 Gereksinimler
Projeyi çalıştırmadan önce sisteminizde şunların kurulu olduğundan emin olun:
* **.NET 8 SDK**
* **SQL Server**
* **Visual Studio 2022** veya **VS Code**
* **Seq** (Log takibi için - *Opsiyonel*)
* **Git** *(projeyi klonlamak için)*
* **Postman** veya **Swagger** *(API testleri için)*
  
### 2. 📥 Projeyi Klonlama
```bash
git clone [https://github.com/Aysegullsahin/FaultReportingAPI](https://github.com/Aysegullsahin/FaultReportingAPI)
cd FaultReportingAPI
```


### 3. ⚙️ Configuration (Connection String)

`appsettings.json` veya `appsettings.Development.json` dosyasında veritabanı bağlantı ayarını kendi ortamınıza göre güncelleyin:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=FaultReportingDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 4. 🗄️ Migration ve Veritabanı Oluşturma

Veritabanını oluşturmak ve güncellemek için aşağıdaki komutları çalıştırın:

#### Migration oluşturma (gerekirse)
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 5. ▶️ Uygulamayı Çalıştırma

Projeyi çalıştırmak için:

```bash
dotnet run
```
veya Visual Studio kullanıyorsanız:

* Startup project’i seçin
* F5 veya Ctrl + F5 ile uygulamayı başlatın

Uygulama çalıştıktan sonra Swagger arayüzüne aşağıdaki adresten erişebilirsiniz:
https://localhost:{port}/swagger

---

### 6. 📊 Logları İzleme (Opsiyonel)
Eğer Seq kullanıyorsanız, uygulama loglarını aşağıdaki adresten takip edebilirsiniz:

### 7.🧰 Kullanılan Teknolojiler ve Kütüphaneler
- .NET 8 Web API
- Entity Framework Core (SQL Server)
- Autofac (Dependency Injection)
- JWT Authentication
- Repository Pattern
- Unit of Work Pattern
- Serilog (Logging)
- Swagger (API Documentation)
- Mapperly (Object Mapping)
- LinqKit (Dynamic Queries)

