using System.Security.Cryptography;

public static class EncryptDecrypt
{
    public static byte[] GenerateKey(string password)
    {
        // Define a static salt
        byte[] salt = new byte[] { };
        const int keySize = 16; // AES-128
        const int iterations = 10000;

        using (var keyGenerator = new Rfc2898DeriveBytes(password, salt, iterations))
        {
            return keyGenerator.GetBytes(keySize);
        }
    }
    public static byte[] Encrypt(string password, byte[] rawData)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = GenerateKey(password);
            aes.GenerateIV();  // IV is generated here
            aes.Padding = PaddingMode.PKCS7;

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            using (var memoryStream = new MemoryStream())
            {
                // Prepend the IV to the encrypted data
                memoryStream.Write(aes.IV, 0, aes.IV.Length);

                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(rawData, 0, rawData.Length);
                    cryptoStream.FlushFinalBlock();
                }
                return memoryStream.ToArray();
            }
        }
    }
    public static byte[] Decrypt(string password, byte[] encryptedData)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = GenerateKey(password);
            aes.Padding = PaddingMode.PKCS7;

            // Extract IV from the beginning of the encrypted data
            byte[] iv = new byte[aes.BlockSize / 8];
            Array.Copy(encryptedData, 0, iv, 0, iv.Length);
            aes.IV = iv;

            using (var decryptor = aes.CreateDecryptor())
            using (var memoryStream = new MemoryStream(encryptedData, iv.Length, encryptedData.Length - iv.Length))
            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            using (var resultStream = new MemoryStream())
            {
                cryptoStream.CopyTo(resultStream);
                return resultStream.ToArray();
            }
        }
    }
}
