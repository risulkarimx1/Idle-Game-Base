using Cysharp.Threading.Tasks;
using Services.DataFramework;

namespace _Scripts.UnitTests.Editor
{
    public class MockEncryptionService: IEncryptionService
    {
        public UniTask<string> EncryptStringAsync(string plainText)
        {
            return UniTask.FromResult(plainText);
        }

        public UniTask<string> DecryptString(string cipherText)
        {
            return UniTask.FromResult(cipherText);
        }
    }
}