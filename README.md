# 🔐 AuthService.API

## `LoginUser` Endpointi Analiz ve Dokümantasyonu

### 📌 Amaç:

`AuthService.API` altında yer alan `LoginUser` endpointi, kullanıcıdan gelen e-posta ve şifre bilgileri ile kimlik doğrulaması yapar. Başarılı doğrulama sonrası:

- Access Token ve Refresh Token üretir,
- Refresh Token'ı Redis'e kaydeder,
- Token'ları yanıt olarak döner.

---

## 🧹 Sorumluluklar ve Katmanlar

| Bağımlılık            | Sorumluluk                                                                        |
| --------------------- | --------------------------------------------------------------------------------- |
| `IUserReadRepository` | Kullanıcı verisini veri kaynağından okumak (e-posta ile kullanıcıyı getirmek).    |
| `IHashingHelper`      | Kullanıcının şifresini doğrulamak için hash ve salt karşılaştırması yapar.        |
| `IJwtTokenGenerator`  | JWT Access Token ve Refresh Token üretir.                                         |
| `IUserRedisService`   | Refresh Token'ı geçici olarak Redis'e kaydeder.                                   |
| `IDateTimeProvider`   | Zamanla ilgili işlemler için dış bağımlılığı soyutlar, test edilebilirlik sağlar. |

---

## 🥪 Adım Adım Akış Analizi

### 1. **Kullanıcının Varlığının Doğrulanması**

```csharp
var user = await _userReadRepository.GetUserRoleByEmailAsync(request.Email);
```

- E-posta ile kullanıcı ve rollerinin yüklü olduğu obje çekilir.
- Kullanıcı yoksa hata dönülür.

### 2. **Şifre Doğrulama**

```csharp
bool passwordCheck = _hashingHelper.VerifyPasswordHash(...);
```

- Gelen şifre, veritabanındaki hash ve salt ile karşılaştırılır.
- Doğru değilse aynı hata mesajı verilir: `"Kullanıcı adı veya şifre hatalı"`. (Güvenlik için kullanıcı var/yok ayrımı yapılmaz.)

### 3. **JWT ve Refresh Token Üretimi**

```csharp
var accessToken = _jwtTokenGenerator.GenerateToken(...);
var refreshToken = _jwtTokenGenerator.GenerateRefreshToken(user.Email);
```

- JWT access token kullanıcı ID, e-posta ve rol içerir.
- Refresh token da üretilir (benzersiz, tahmin edilmesi zor ve süreli).

### 4. **Refresh Token'ın Redis'e Kaydı**

```csharp
var key = $"refreshToken:{user.Email}";
await _userRedisService.SetAsync(key, refreshToken.Token, expiration);
```

- Token, `refreshToken:{email}` key formatıyla Redis'e kaydedilir.
- Redis kullanımı: merkezi olmayan erişim, performans ve token geçersizleştirme avantajı sağlar.

### 5. **Başarılı Yanıt Dönülmesi**

```csharp
return Result<LoginUserResponse>.Success(...);
```

- Access ve Refresh Token içeren DTO dönülür.

### 6. **Hata Yönetimi**

```csharp
catch (Exception ex)
{
    return Result<LoginUserResponse>.Failure("Serviste hata meydana geldi");
}
```

- Hatalar loglanmalı (mevcutta eksik), dışarıya genel mesaj verilir.

---

## ✅ Güçlü Yönler

- **SOLID’e Uygunluk**: Bağımlılıklar soyutlandığı için sorumluluklar net.
- **Test Edilebilirlik**: DI sayesinde unit test yazımı kolay.
- **Güvenlik**: Hash/salt şifre kontrolü, token bazlı oturum yönetimi.
- **Performans**: Redis ile refresh token'ların hızlı saklanması.

---

## ❗ İyileştirme Önerileri

### 🔍 1. **Loglama Eksik**

```csharp
_logger.LogError(ex, "Login işlemi sırasında hata meydana geldi.");
```

- Hatalar için log sistemi entegrasyonu eksik.

### 🧪 2. **Brute Force / Rate Limiting**

- Redis ile IP veya e-posta bazlı deneme sınırlama eklenmeli.

### 📌 3. **Refresh Token Çoğu Oturum Senaryosu**

- `refreshToken:{email}` yapısı aynı e-posta için tek oturuma izin verir.
- `refreshToken:{email}:{deviceId}` yapısı kullanılabilir.

### 🔐 4. **Token Yapılandırması**

- Token expire süreleri kodda sabit değil, config dosyasından alınmalı.
- JWT claim içeriği genişletilebilir (tenantId, firmaId vb.)

---

## 🧾 Özet

`AuthService.API` altındaki `LoginUser` endpointi, modern yazılım prensiplerine uygun olarak tasarlanmış ve uygulamaya hazır bir giriş işlemi altyapısı sunar. Doğru bir şekilde katmanlandırılmış, Redis gibi cache yapılarından faydalanılmış ve kolay geliştirilebilir/test edilebilir yapıdadır.

Eksik olan alanlar: loglama, rate-limiting, token çoklu oturum senaryoları ve konfigurasyon odaklı expire yönetimidir.

---

## `RefreshToken` Endpointi Analiz ve Dokümantasyonu

### 📌 Amaç:

`RefreshToken` endpointi, kullanıcıdan gelen refresh token bilgisini kullanarak yeni bir access token ve refresh token üretir. Böylece kullanıcı oturumu, access token süresi dolduğunda kesintiye uğramadan devam eder.

---

## 🧹 Sorumluluklar ve Katmanlar

|                       |                                                    |
| --------------------- | -------------------------------------------------- |
| Bağımlılık            | Sorumluluk                                         |
| `IJwtTokenValidator`  | Refresh token'dan kullanıcı bilgilerini çıkarır.   |
| `IMediator`           | `RefreshTokenUserRequest` ile iş akışını başlatır. |
| `IUserReadRepository` | E-posta ile kullanıcıyı getirir.                   |
| `IUserRedisService`   | Refresh Token geçerliliğini Redis'te kontrol eder. |
| `IJwtTokenGenerator`  | Yeni Access Token ve Refresh Token üretir.         |
| `IDateTimeProvider`   | Token süresi hesaplamalarında zaman sağlar.        |

---

## 🥪 Adım Adım Akış Analizi

### 1. **Refresh Token’ın Cookie’den Alınması**

```
var refreshToken = Request.Cookies["refreshToken"];
```

- Kullanıcının cookie'sinden refresh token alınır.
- Boşsa: `401 Unauthorized` dönülür.

### 2. **Refresh Token Doğrulama ve E-posta Çözümleme**

```
var userClaims = _jwtTokenValidator.GetPrincipalFromRefreshToken(refreshToken);
```

- Refresh token parse edilir ve içindeki email bilgisi alınır.
- E-posta alınamazsa: `401 Unauthorized` dönülür.

### 3. **RefreshTokenUserRequest Gönderimi**

```
await _mediator.Send(new RefreshTokenUserRequest { Email = userEmail });
```

- Refresh token doğrulandıktan sonra ilgili handler çağrılır.

### 4. **Redis’te Refresh Token Geçerliliği Kontrolü**

```
await _userRedisService.IsExistRefreshTokenByEmail(request.Email);
```

- Eğer refresh token kayıtlı değilse, geçersiz kabul edilir.

### 5. **Yeni Token’ların Üretilmesi**

```
var newAccessToken = _jwtTokenGenerator.GenerateToken(...);
var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken(...);
```

- Hem access hem de refresh token yeniden üretilir.

### 6. **Redis’e Refresh Token Yazılması**

```
await _userRedisService.SetAsync(key, newRefreshToken.Token, expiration);
```

- Yeni refresh token, Redis’e eskiyi geçersiz kılacak şekilde yazılır.

### 7. **Yeni Token’ların Dönülmesi ve Cookie’ye Yazılması**

```
Response.Cookies.Append("refreshToken", result.Value.RefreshToken, new CookieOptions { ... });
```

- Refresh token cookie olarak döner.
- Access token ise JSON body içinde döner.

---

## ✅ Güçlü Yönler

- **Stateless API yapısı korunur** (Tokenlar ile kimlik doğrulama).
- **Refresh token kontrolü merkezi yapılır** (Redis).
- **Yeni token üretimi backend tarafından yapılır** (güvenli).

---

## ❗ İyileştirme Önerileri

- **Token Çoklu Oturum Yönetimi**: Tek refresh token yerine, cihaz bazlı kayıtlar düşünülmeli.
- **Refresh Token Geçersizleştirme**: Logout işlemi ile Redis kaydı silinmeli.
- **Loglama Eksikliği**: Başarısız refresh denemeleri loglanmalı.
- **Token Sürelerinin Konfigüre Edilmesi**: Kod yerine yapılandırma dosyasından alınmalı.

---

## 🧾 Özet

`RefreshToken` endpointi, kullanıcı deneyimini kesintiye uğratmadan yeni token’lar üretmekte ve güvenli oturum devamlılığı sağlamaktadır. Redis ile geçerlilik kontrolü, token invalidation ve erişim yönetimi doğru şekilde ele alınmıştır.

Eksik yönler: çoklu oturum desteği, loglama, yapılandırılabilir expire süreleri ve refresh token geçersizleştirme senaryolarıdır.

---

## `RegisterUser` Endpointi Analiz ve Dokümantasyonu

### 📌 Amaç:

`RegisterUser` endpointi, yeni bir kullanıcının kaydını gerçekleştirerek Access ve Refresh Token oluşturur. Aşağıdaki işlemleri gerçekleştirir:

- Eğer e-posta sistemde varsa kayıt engellenir,
- Parola hash ve salt üretilir,
- Kullanıcı verisi kaydedilir,
- Kullanıcıya varsayılan olarak `Customer` rolü atanır,
- Access ve Refresh token oluşturulur,
- Refresh Token Redis'e kaydedilir,
- Kullanıcıya yanıt olarak token'lar dönülür.

---

## 🥪 Sorumluluklar ve Katmanlar

| Bağımlılık                           | Sorumluluk                                                      |
| ------------------------------------ | --------------------------------------------------------------- |
| `IUserReadRepository`                | Kullanıcının daha önceden kayıtlı olup olmadığını kontrol eder. |
| `IUserWriteRepository`               | Yeni kullanıcı kaydı yapar.                                     |
| `IOperationClaimReadRepository`      | Sistem rolleri arasından `Customer` rolünü alır.                |
| `IUserOperationClaimWriteRepository` | Yeni kullanıcıya `Customer` rolünü atar.                        |
| `IHashingHelper`                     | Parola hash/salt oluşturur.                                     |
| `IJwtTokenGenerator`                 | Token oluşturur.                                                |
| `IDateTimeProvider`                  | Tarih/expire hesapları için zaman bilgisi sağlar.               |
| `IUserRedisService`                  | Refresh token'ı Redis'e yazar.                                  |
| `IUnitOfWork`                        | Değişikliklerin veritabanına kaydından sorumludur.              |

---

## 🥪 Akış Detayı

### 1. **E-posta Kontrolü**

```csharp
var user = await _userReadRepository.GetUserRoleByEmailAsync(request.Email);
```

- Eğer kullanıcı zaten varsa hata dönülür.

### 2. **Parola Hash ve Kullanıcı Kaydı**

```csharp
_hashingHelper.CreatePasswordHash(...);
await _userWriteRepository.AddAsync(newUser);
```

- Parola hashlenir ve kullanıcı oluşturulur.

### 3. **Rol Atama**

```csharp
var customerRoleId = ...;
await _userOperationClaimWriteRepository.AddAsync(...);
```

- Yeni kullanıcıya varsayılan rol (Customer) atanır.

### 4. **Access ve Refresh Token Oluşturma**

```csharp
var accessToken = _jwtTokenGenerator.GenerateToken(...);
var refreshToken = _jwtTokenGenerator.GenerateRefreshToken(...);
```

### 5. **Redis'e Refresh Token Yazılması**

```csharp
await _userRedisService.SetAsync(key, refreshToken.Token, expiration);
```

- Refresh token Redis'e yazılır.

### 6. **Veritabanına Kaydetme ve Yanıt**

```csharp
await _unitOfWork.SaveChangesAsync();
return Result<RegisterUserResponse>.Success(...);
```

---

## ✅ Güçlü Yönler

- Katmanlı mımari
- Roller için ayrı katman
- Hash + Salt parola yapısı
- Token bazlı authentication mimarisi
- Redis kullanımı

---

## ❗️ İyileştirme Önerileri

- **Loglama eksik**: Kayıt hataları loglanmıyor.
- **Email onay mekanizması yok**: Doğrulama kodu gönderimi yok.
- **Expire ve claim config dosyasından okunmuyor**.
- **Refresh Token device bazlı değil**: Aynı e-posta ile birden fazla oturum desteklenmiyor.

---

## 📆 Request Örneği

```json
POST /api/users/register
Content-Type: application/json

{
  "firstName": "Ahmet",
  "lastName": "Yılmaz",
  "email": "ahmet@example.com",
  "password": "123456"
}
```

## 📆 Response Örneği

```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "c4a7c8bd-a0ff-4ff2-8c7a-3efb5ec5f379",
  "userId": 42,
  "email": "ahmet@example.com",
  "fullName": "Ahmet Yılmaz"
}
```
