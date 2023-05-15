using EncryptDecryptLibray;
using System.Security.Cryptography;

namespace secondMicroServer.Controllers
{
    public static class SecondServerControllerFunctions
    {
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
        public static string Decrypt(byte[] encryptedData)
        {
            byte[] key, iv;
            string fileName = @"C:\Users\\vicos\Desktop\work\workWithRoi\EncrypDecrypProject\KeyAndIv.json";
            EncryptDecrypt.ReadKeyAndIvFromFile(out key, out iv, ref fileName);
            return DecryptingData(encryptedData, key, iv);
        }
    }
}
