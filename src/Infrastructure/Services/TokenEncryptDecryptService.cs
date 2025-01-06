using System.Security.Cryptography;
using System.Text;
using ASD.Onboard.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ASD.Onboard.Infrastructure.Services;

internal sealed class TokenEncryptDecryptService(IConfiguration configuration): ITokenEncrypDecryptService
{
    public string EncryptToken(string token)
    {
        var key = configuration.GetValue<string>("EncryptionKey"); // Store securely, e.g., in Azure Key Vault
        if (string.IsNullOrEmpty(key) || key.Length != 32)
        {
            throw new InvalidOperationException("EncryptionKey must be a 32-character string.");
        }

        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(key);
        aes.GenerateIV(); // Generate a new IV for each encryption

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        {
            using (var writer = new StreamWriter(cs))
            {
                writer.Write(token);
            }
        }

        var iv = Convert.ToBase64String(aes.IV);
        var encryptedToken = Convert.ToBase64String(ms.ToArray());

        // Include the IV with the encrypted token
        return $"{iv}:{encryptedToken}";
    }
    public string DecryptToken(string encryptedToken)
    {
        var key = configuration.GetValue<string>("EncryptionKey");
        if (string.IsNullOrEmpty(key) || key.Length != 32)
        {
            throw new InvalidOperationException("EncryptionKey must be a 32-character string.");
        }

        var parts = encryptedToken.Split(':');
        if (parts.Length != 2)
        {
            throw new ArgumentException("Invalid encrypted token format");
        }

        var iv = Convert.FromBase64String(parts[0]);
        var cipherText = Convert.FromBase64String(parts[1]);

        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(key);
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(cipherText);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var reader = new StreamReader(cs);

        return reader.ReadToEnd();
    }


}
