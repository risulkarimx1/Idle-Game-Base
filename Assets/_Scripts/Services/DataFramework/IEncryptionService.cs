using Cysharp.Threading.Tasks;

namespace Services.DataFramework
{
    public interface IEncryptionService
    {
        UniTask<string> EncryptStringAsync(string plainText);
        UniTask<string> DecryptString(string cipherText);
    }
}