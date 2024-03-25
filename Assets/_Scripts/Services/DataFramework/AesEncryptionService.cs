using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Cysharp.Threading.Tasks;
using GameCode.Init;
using GameCode.Utils;

namespace Services.DataFramework
{
    public class AesEncryptionService : IEncryptionService
    {
        public async UniTask<string> EncryptStringAsync(string plainText)
        {
            using var aesAlg = Aes.Create();
            aesAlg.Key = Encoding.UTF8.GetBytes(GameConfig.DataKey);
            aesAlg.IV = Encoding.UTF8.GetBytes(GameConfig.DataInitVector);

            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    await swEncrypt.WriteAsync(plainText);
                }
            }

            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        public async UniTask<string> DecryptString(string cipherText)
        {
            var cipherBytes = Convert.FromBase64String(cipherText);

            using var aesAlg = Aes.Create();
            aesAlg.Key = Encoding.UTF8.GetBytes(GameConfig.DataKey);
            aesAlg.IV = Encoding.UTF8.GetBytes(GameConfig.DataInitVector);

            var decrypted = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using var msDecrypt = new MemoryStream(cipherBytes);
            await using var csDecrypt = new CryptoStream(msDecrypt, decrypted, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            var data = await srDecrypt.ReadToEndAsync();
            return data;
        }
    }
}