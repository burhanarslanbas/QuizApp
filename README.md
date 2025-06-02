# QuizApp

Bu proje, QuizApp uygulamasının backend API'sini ve Frontend yapısını içerir.

## Gereksinimler

- .NET 8.0 SDK
- Visual Studio 2022 veya Visual Studio Code
- SQL Server (LocalDB veya tam sürüm)

## Kurulum

1. Projeyi klonlayın:
```bash
git clone https://github.com/[kullanıcı-adı]/QuizApp.git
cd QuizApp
```

2. Bağımlılıkları yükleyin:
```bash
dotnet restore
```

3. Veritabanını oluşturun:
```bash
dotnet ef database update
```

4. Uygulamayı çalıştırın:
```bash
dotnet run --project Presentation/QuizApp.API
```

## API Dokümantasyonu

Swagger UI üzerinden API dokümantasyonuna erişebilirsiniz:
- Development ortamında: https://localhost:7001/swagger
- HTTP üzerinden: http://localhost:5000/swagger

## Proje Yapısı

- `QuizApp.API`: API katmanı
- `QuizApp.Application`: Uygulama katmanı (DTOs, Interfaces, Services)
- `QuizApp.Domain`: Domain katmanı (Entities, Enums)
- `QuizApp.Infrastructure`: Altyapı katmanı (DbContext, Migrations, Handlers)
- `QuizApp.Tests`: Test projesi

## Geliştirme

1. Yeni bir branch oluşturun:
```bash
git checkout -b feature/yeni-ozellik
```

2. Değişikliklerinizi commit edin:
```bash
git add .
git commit -m "Yeni özellik eklendi"
```

3. Branch'inizi push edin:
```bash
git push origin feature/yeni-ozellik
```

## Lisans

Bu proje MIT lisansı altında lisanslanmıştır.
