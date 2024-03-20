using Cysharp.Threading.Tasks;

namespace Frameworks.DataFramework
{
    public interface IEncryptionService
    {
        UniTask<string> EncryptStringAsync(string plainText);
        UniTask<string> DecryptString(string cipherText);
    }
}
