# Project Setup Guide

Bu doküman, .NET Core mikroservis projesini local ortamda ve Docker üzerinde ayağa kaldırmak için gerekli adımları içerir.

---

## 1. Ortam Değişkenleri

`appsettings.Development.json` içinde güncellenmesi gereken değişkenler:

```json
{
  "ConnectionStrings": {
    "SqlConnection": "Server=localhost;Database=ETrade.AuthServiceDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "Secret": "YOUR_SUPER_SECRET_KEY_HERE_1234567890",
    "Issuer": "AuthService",
    "Audience": "ETradeApp",
    "ExpiryMinutes": 60,
    "ExpiryDays": 7
  },
  "Redis": {
    "ConnectionString": "localhost:6379"
  }
}
```

## 2. Docker'da Yapılması Gerekenler

docker run -d --name myredis -p 6379:6379 -v redis_data:/data redis:7 redis-server --appendonly yes
docker exec -it myredis redis-cli ping 
