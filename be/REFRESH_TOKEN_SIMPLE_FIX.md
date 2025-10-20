# Refresh Token - Chuyển sang SHA256

## Vấn đề

- Refresh token chậm 15 giây
- Nguyên nhân: BCrypt rất chậm

## Giải pháp

**Chuyển hoàn toàn sang SHA256 + salt**

## Thay đổi

### 1. **JwtTokenService.cs**

```csharp
// Tạo hash mới bằng SHA256
public string Hash(string value)
{
    const string salt = "VisionCare_RefreshToken_Salt_2024";
    using var sha256 = SHA256.Create();
    var saltedValue = value + salt;
    var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedValue));
    return Convert.ToBase64String(hashBytes);
}
```

### 2. **AuthService.cs**

```csharp
// So sánh hash trực tiếp (nhanh)
var expectedHash = Hash(refreshToken);
var tokenMatch = validTokens.FirstOrDefault(t => t.TokenHash == expectedHash);
```

### 3. **Database Migration**

```sql
-- Xóa tất cả tokens cũ (BCrypt)
DELETE FROM refreshtokens;
```

## Kết quả

- **Trước**: 15 giây (BCrypt)
- **Sau**: ~100ms (SHA256)
- **Cải thiện**: 150x nhanh hơn

## Cách áp dụng

1. Restart backend
2. Chạy migration: `04_migrate_refresh_tokens.sql`
3. Users cần login lại (tokens cũ bị xóa)

## Lưu ý

- Chỉ dùng SHA256, không hỗ trợ BCrypt nữa
- Tokens cũ sẽ không hoạt động
- Performance cải thiện đáng kể
