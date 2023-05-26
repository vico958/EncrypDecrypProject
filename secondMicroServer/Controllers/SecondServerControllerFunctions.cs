using EncryptDecryptLibray;
using StackExchange.Redis;
using System.Security.Cryptography;
using System.Text;

namespace secondMicroServer.Controllers
{
    public static class SecondServerControllerFunctions
    {
        private static ConfigurationOptions redisConfig = ConfigurationOptions.Parse("localhost:6379");
        private static ConnectionMultiplexer redisConnection = ConnectionMultiplexer.Connect(redisConfig);
        private static string DecryptingData(byte[] encryptedData, byte[] key, byte[] iv)
        {
            string decryptedData = null;
            using (Aes aes = Aes.Create())
            {
                ICryptoTransform cryptoDecryptor = aes.CreateDecryptor(key, iv);
                using (MemoryStream memoryStream = new MemoryStream(encryptedData))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoDecryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            decryptedData = streamReader.ReadToEnd();
                        }
                    }
                }
            }
            return decryptedData;
        }
        public static string Decrypt()
        {
            byte[] key, iv;
            string fileName = @"C:\Users\\vicos\Desktop\work\workWithRoi\EncrypDecrypProject\KeyAndIv.json";
            EncryptDecrypt.ReadKeyAndIvFromFile(out key, out iv, ref fileName);
            var redisDatabase = redisConnection.GetDatabase();
            byte[] encryptedData = redisDatabase.StringGet("myEncryptedMessage");
            var decryptedData = DecryptingData(encryptedData, key, iv);
            redisDatabase.StringSet("myDcryptedMessage", decryptedData);
            return decryptedData;
        }
    }
}
