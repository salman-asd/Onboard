namespace ASD.Onboard.Application.Common.Interfaces;

public interface ITokenEncrypDecryptService
{
    string EncryptToken(string token);
    string DecryptToken(string encryptedToken);
}
