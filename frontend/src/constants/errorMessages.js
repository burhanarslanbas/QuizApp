export const ERROR_MESSAGES = {
  // Genel hatalar
  NETWORK_ERROR: 'Sunucuya bağlanılamıyor. Lütfen internet bağlantınızı kontrol edin.',
  SERVER_ERROR: 'Bir hata oluştu. Lütfen daha sonra tekrar deneyin.',
  UNKNOWN_ERROR: 'Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyin.',

  // Kimlik doğrulama hataları
  AUTH: {
    INVALID_CREDENTIALS: 'Kullanıcı adı veya şifre hatalı.',
    USER_NOT_FOUND: 'Kullanıcı bulunamadı.',
    ACCOUNT_LOCKED: 'Hesabınız kilitlendi. Lütfen daha sonra tekrar deneyin.',
    SESSION_EXPIRED: 'Oturumunuz sona erdi. Lütfen tekrar giriş yapın.',
    UNAUTHORIZED: 'Bu işlem için yetkiniz bulunmuyor.',
    INVALID_TOKEN: 'Geçersiz oturum. Lütfen tekrar giriş yapın.',
  },

  // Kayıt hataları
  REGISTER: {
    USERNAME_EXISTS: 'Bu kullanıcı adı zaten kullanılıyor.',
    EMAIL_EXISTS: 'Bu e-posta adresi zaten kullanılıyor.',
    WEAK_PASSWORD: 'Şifre çok zayıf. Lütfen daha güçlü bir şifre seçin.',
    INVALID_EMAIL: 'Geçersiz e-posta adresi.',
    PASSWORD_MISMATCH: 'Şifreler eşleşmiyor.',
  },

  // Form validasyon hataları
  VALIDATION: {
    REQUIRED_FIELD: 'Bu alan zorunludur.',
    INVALID_FORMAT: 'Geçersiz format.',
    MIN_LENGTH: 'En az {min} karakter olmalıdır.',
    MAX_LENGTH: 'En fazla {max} karakter olmalıdır.',
  }
}; 