# Quiz App API Dokümantasyonu

## Genel Bilgiler

- Base URL: `https://api.quizapp.com`
- Tüm istekler JSON formatında yapılmalıdır
- Kimlik doğrulama için JWT token kullanılmaktadır
- Tüm isteklerde `Authorization` header'ı gerekli olabilir

## Kimlik Doğrulama

### Giriş Yapma

```http
POST /api/auth/login
```

#### İstek
```json
{
  "email": "user@example.com",
  "password": "password123"
}

```

#### Başarılı Yanıt
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": "1",
    "email": "user@example.com",
    "name": "John Doe",
    "role": "student"
  }
}
```

### Token Yenileme

```http
POST /api/auth/refresh
```

#### İstek
```json
{
  "refreshToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

## Quiz İşlemleri

### Quiz Listesi

```http
GET /api/quizzes
```

#### Query Parametreleri
- `category` (opsiyonel): Kategori ID'si
- `search` (opsiyonel): Arama terimi
- `sort` (opsiyonel): Sıralama kriteri (newest, oldest, title, popular)
- `page` (opsiyonel): Sayfa numarası
- `limit` (opsiyonel): Sayfa başına öğe sayısı

#### Başarılı Yanıt
```json
{
  "items": [
    {
      "id": "1",
      "title": "Matematik Quiz",
      "description": "Temel matematik soruları",
      "categoryId": "1",
      "duration": 30,
      "passingScore": 60,
      "questionCount": 10,
      "participantCount": 150,
      "createdAt": "2024-03-15T10:00:00Z"
    }
  ],
  "total": 100,
  "page": 1,
  "limit": 10
}
```

### Quiz Detayı

```http
GET /api/quizzes/{id}
```

#### Başarılı Yanıt
```json
{
  "id": "1",
  "title": "Matematik Quiz",
  "description": "Temel matematik soruları",
  "categoryId": "1",
  "duration": 30,
  "passingScore": 60,
  "questions": [
    {
      "id": "1",
      "text": "2 + 2 = ?",
      "options": ["3", "4", "5", "6"],
      "correctAnswers": [1],
      "points": 1,
      "difficulty": "easy"
    }
  ],
  "createdAt": "2024-03-15T10:00:00Z",
  "updatedAt": "2024-03-15T10:00:00Z"
}
```

### Quiz Oluşturma

```http
POST /api/quizzes
```

#### İstek
```json
{
  "title": "Yeni Quiz",
  "description": "Quiz açıklaması",
  "categoryId": "1",
  "duration": 30,
  "passingScore": 60,
  "questions": [
    {
      "text": "Soru metni",
      "options": ["A", "B", "C", "D"],
      "correctAnswers": [0],
      "points": 1,
      "difficulty": "medium"
    }
  ]
}
```

### Quiz Güncelleme

```http
PUT /api/quizzes/{id}
```

#### İstek
```json
{
  "title": "Güncellenmiş Quiz",
  "description": "Güncellenmiş açıklama",
  "categoryId": "1",
  "duration": 45,
  "passingScore": 70,
  "questions": [
    {
      "id": "1",
      "text": "Güncellenmiş soru",
      "options": ["A", "B", "C", "D"],
      "correctAnswers": [0],
      "points": 2,
      "difficulty": "hard"
    }
  ]
}
```

### Quiz Silme

```http
DELETE /api/quizzes/{id}
```

## Soru İşlemleri

### Soru Ekleme

```http
POST /api/quizzes/{id}/questions
```

#### İstek
```json
{
  "text": "Yeni soru",
  "options": ["A", "B", "C", "D"],
  "correctAnswers": [0],
  "points": 1,
  "difficulty": "medium"
}
```

### Soru Güncelleme

```http
PUT /api/quizzes/{id}/questions/{questionId}
```

#### İstek
```json
{
  "text": "Güncellenmiş soru",
  "options": ["A", "B", "C", "D"],
  "correctAnswers": [0],
  "points": 2,
  "difficulty": "hard"
}
```

### Soru Silme

```http
DELETE /api/quizzes/{id}/questions/{questionId}
```

## Sonuç İşlemleri

### Quiz Çözme

```http
POST /api/quizzes/{id}/submit
```

#### İstek
```json
{
  "answers": [
    {
      "questionId": "1",
      "selectedOption": 0
    }
  ],
  "timeSpent": 1800
}
```

#### Başarılı Yanıt
```json
{
  "id": "1",
  "score": 80,
  "passed": true,
  "correctAnswers": 8,
  "timeSpent": 1800,
  "completedAt": "2024-03-15T10:30:00Z"
}
```

### Sonuç Detayı

```http
GET /api/quizzes/{id}/results
```

#### Başarılı Yanıt
```json
{
  "id": "1",
  "score": 80,
  "passed": true,
  "correctAnswers": 8,
  "timeSpent": 1800,
  "completedAt": "2024-03-15T10:30:00Z",
  "answers": [
    {
      "questionId": "1",
      "questionText": "Soru metni",
      "selectedOption": 0,
      "correctOption": 0,
      "isCorrect": true,
      "points": 1
    }
  ]
}
```

### Quiz İstatistikleri

```http
GET /api/quizzes/{id}/stats
```

#### Başarılı Yanıt
```json
{
  "totalParticipants": 150,
  "averageScore": 75.5,
  "passRate": 80.0,
  "averageTime": 1800,
  "questionStats": [
    {
      "questionId": "1",
      "correctRate": 85.0,
      "averageTime": 120
    }
  ],
  "scoreDistribution": [
    {
      "range": "0-20",
      "count": 5
    }
  ],
  "timeDistribution": [
    {
      "range": "0-10",
      "count": 10
    }
  ]
}
```

## Hata Kodları

- `400 Bad Request`: Geçersiz istek
- `401 Unauthorized`: Kimlik doğrulama gerekli
- `403 Forbidden`: Yetkisiz erişim
- `404 Not Found`: Kaynak bulunamadı
- `409 Conflict`: Çakışma durumu
- `500 Internal Server Error`: Sunucu hatası

## Hata Yanıtı Formatı

```json
{
  "error": {
    "code": "QUIZ_NOT_FOUND",
    "message": "Quiz bulunamadı",
    "details": "Belirtilen ID'ye sahip quiz mevcut değil"
  }
}
``` 