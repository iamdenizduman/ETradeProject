# 🔐 AuthService.API

## `LoginUser` Endpointi

### 📌 Amaç:

`AuthService.API` altında yer alan `LoginUser` endpointi, kullanıcıdan gelen e-posta ve şifre bilgileri ile kimlik doğrulaması yapar. Başarılı doğrulama sonrası:

- Access Token ve Refresh Token üretir,
- Refresh Token'ı Redis'e kaydeder,
- Token'ları yanıt olarak döner.

## 📆 Request Örneği

```json
POST /api/users/register
Content-Type: application/json

{
  "email": "denizdumanresmi@gmail.com",
  "password": "deniz123"
}
```

## 📆 Response Örneği

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI1MDc5YjRlZS1lYzNmLTQ3NzEtNDg1ZS0wOGRkZTBmZjQyNzgiLCJlbWFpbCI6ImRlbml6ZHVtYW5yZXNtaUBnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJDdXN0b21lciIsImp0aSI6IjY4ZmU3YzU3LWFkNjgtNDhkOC04OWRkLTBlMGYwNmUzYWE1OCIsIm5iZiI6MTc1NzUzODkyMywiZXhwIjoxNzU3NTQyNTIzLCJpc3MiOiJBdXRoU2VydmljZSIsImF1ZCI6IkVUcmFkZUFwcCJ9.TaQeF_x1le3uBkEVc9pYUOCGa9YaqqBmkcdNzE4NuI0"
}
```

## `RefreshToken` Endpointi

### 📌 Amaç:

`AuthService.API` altında yer alan `RefreshToken` endpointi, client'ta var olan kullanıcıya ait cookie'deki refresh token ile gelir. Başarılı doğrulama sonrası:

- Access Token ve Refresh Token üretir,
- Refresh Token'ı Redis'e kaydeder,
- Token'ları yanıt olarak döner.

## 📆 Request Örneği

```json
POST /api/users/register
Content-Type: application/json
Cookie: refreshTokenValue

{
  // cookie'den değeri alıyor
}
```

## 📆 Response Örneği

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI1MDc5YjRlZS1lYzNmLTQ3NzEtNDg1ZS0wOGRkZTBmZjQyNzgiLCJlbWFpbCI6ImRlbml6ZHVtYW5yZXNtaUBnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJDdXN0b21lciIsImp0aSI6IjY4ZmU3YzU3LWFkNjgtNDhkOC04OWRkLTBlMGYwNmUzYWE1OCIsIm5iZiI6MTc1NzUzODkyMywiZXhwIjoxNzU3NTQyNTIzLCJpc3MiOiJBdXRoU2VydmljZSIsImF1ZCI6IkVUcmFkZUFwcCJ9.TaQeF_x1le3uBkEVc9pYUOCGa9YaqqBmkcdNzE4NuI0"
}
```
