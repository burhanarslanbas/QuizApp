# Quiz App

Modern ve kullanıcı dostu bir quiz uygulaması. Öğretmenler quiz oluşturabilir, öğrenciler quiz çözebilir ve sonuçları analiz edebilir.

## Özellikler

### Genel Özellikler
- 🎯 Rol tabanlı erişim kontrolü (Öğrenci, Öğretmen, Admin)
- 📱 Responsive tasarım
- 🔒 Güvenli kimlik doğrulama
- 🌐 Modern ve kullanıcı dostu arayüz

### Öğrenci Özellikleri
- 📝 Quiz çözme
- ⏱️ Süre takibi
- 📊 Sonuç analizi
- 🏆 Başarı istatistikleri

### Öğretmen Özellikleri
- ✏️ Quiz oluşturma ve düzenleme
- 📊 Detaylı istatistikler
- 👥 Öğrenci performans analizi
- 📈 Başarı oranı takibi

### Admin Özellikleri
- 👥 Kullanıcı yönetimi
- 📚 Kategori yönetimi
- 🔧 Sistem ayarları
- 📊 Genel istatistikler

## Teknolojiler

### Frontend
- React
- Material-UI
- Redux Toolkit
- React Router
- Axios
- Recharts

### Backend
- .NET Core
- Entity Framework Core
- SQL Server
- JWT Authentication

## Kurulum

### Gereksinimler
- Node.js (v14 veya üzeri)
- .NET Core SDK (v6.0 veya üzeri)
- SQL Server
- Visual Studio 2022 (önerilen)

### Frontend Kurulumu
```bash
# Proje dizinine git
cd frontend

# Bağımlılıkları yükle
npm install

# Geliştirme sunucusunu başlat
npm start
```

### Backend Kurulumu
```bash
# Proje dizinine git
cd backend

# Bağımlılıkları geri yükle
dotnet restore

# Veritabanını oluştur
dotnet ef database update

# Uygulamayı başlat
dotnet run
```

## Kullanım

### Öğrenci Olarak
1. Quiz listesinden bir quiz seçin
2. Quiz'i başlatın
3. Soruları cevaplayın
4. Sonuçlarınızı görüntüleyin

### Öğretmen Olarak
1. "Yeni Quiz" butonuna tıklayın
2. Quiz bilgilerini girin
3. Soruları ekleyin
4. Quiz'i yayınlayın
5. Sonuçları analiz edin

### Admin Olarak
1. Kullanıcıları yönetin
2. Kategorileri düzenleyin
3. Sistem ayarlarını yapılandırın
4. Genel istatistikleri görüntüleyin

## API Dokümantasyonu

### Kimlik Doğrulama
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "string",
  "password": "string"
}
```

### Quiz İşlemleri
```http
GET /api/quizzes
GET /api/quizzes/{id}
POST /api/quizzes
PUT /api/quizzes/{id}
DELETE /api/quizzes/{id}
```

### Soru İşlemleri
```http
GET /api/quizzes/{id}/questions
POST /api/quizzes/{id}/questions
PUT /api/quizzes/{id}/questions/{questionId}
DELETE /api/quizzes/{id}/questions/{questionId}
```

### Sonuç İşlemleri
```http
POST /api/quizzes/{id}/submit
GET /api/quizzes/{id}/results
GET /api/quizzes/{id}/stats
```

## Katkıda Bulunma

1. Bu depoyu fork edin
2. Yeni bir branch oluşturun (`git checkout -b feature/amazing-feature`)
3. Değişikliklerinizi commit edin (`git commit -m 'Add some amazing feature'`)
4. Branch'inizi push edin (`git push origin feature/amazing-feature`)
5. Bir Pull Request oluşturun

## Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için [LICENSE](LICENSE) dosyasına bakın.

## İletişim

Proje Sahibi - [@github](https://github.com/yourusername)

Proje Linki: [https://github.com/yourusername/quiz-app](https://github.com/yourusername/quiz-app)
