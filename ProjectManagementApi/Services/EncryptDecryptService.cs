using ProjectManagementApi.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace ProjectManagementApi.Services
{
    public class EncryptDecryptService : IEncryptDecryptService
    {
        private Aes Algorithm { get; set; }
        private byte[] Key { get; set; }
        private byte[] IniVetor { get; set; }

        public EncryptDecryptService(IConfiguration configuration)
        {
            this.Algorithm = Aes.Create();

            Key = Encoding.ASCII.GetBytes(configuration.GetSection("Keys")["EncryptKey"]);
            IniVetor = Encoding.ASCII.GetBytes(configuration.GetSection("Keys")["EncryptIV"]);
        }

        public string Encrypt(string simpletext)
        {
            byte[] symEncryptedData;

            var dataToProtectAsArray = Encoding.UTF8.GetBytes(simpletext);
            using (var encryptor = this.Algorithm.CreateEncryptor(Key, IniVetor))
            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                cryptoStream.Write(dataToProtectAsArray, 0, dataToProtectAsArray.Length);
                cryptoStream.FlushFinalBlock();
                symEncryptedData = memoryStream.ToArray();
            }
            this.Algorithm.Dispose();
            return Convert.ToBase64String(symEncryptedData);
        }

        public string Decrypt(string entryText)
        {
            var symEncryptedData = Convert.FromBase64String(entryText);
            byte[] symUnencryptedData;
            using (var decryptor = this.Algorithm.CreateDecryptor(Key, IniVetor))
            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write))
            {
                cryptoStream.Write(symEncryptedData, 0, symEncryptedData.Length);
                cryptoStream.FlushFinalBlock();
                symUnencryptedData = memoryStream.ToArray();
            }
            this.Algorithm.Dispose();
            return System.Text.Encoding.Default.GetString(symUnencryptedData);
        }
    }
}
