using System.Security.Cryptography;
using TrackManagement.Application.Interfaces.Services;

namespace TrackManagement.Infrastructure.Auth;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100_000;
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

        var combined = new byte[SaltSize + HashSize];
        Buffer.BlockCopy(salt, 0, combined, 0, SaltSize);
        Buffer.BlockCopy(hash, 0, combined, SaltSize, HashSize);

        return Convert.ToBase64String(combined);
    }

    public bool Verify(string password, string passwordHash)
    {
        byte[] combined;
        try
        {
            combined = Convert.FromBase64String(passwordHash);
        }
        catch (FormatException)
        {
            return false;
        }

        if (combined.Length != SaltSize + HashSize)
        {
            return false;
        }

        var salt = combined[..SaltSize];
        var expectedHash = combined[SaltSize..];
        var actualHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

        return CryptographicOperations.FixedTimeEquals(expectedHash, actualHash);
    }
}
