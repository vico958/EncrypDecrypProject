using Newtonsoft.Json;
using System.Text;
using EncryptDecryptLibray;
using System.Security.Cryptography;

namespace firstMicroServer.Controllers
{
    public static class FirstServerControllerFunctions
    {
        private static string url = "https://localhost:7170/";
        private static readonly HttpClient client = new HttpClient();
        public static async Task<string> EncrypMessageAsync(string message)
        {
            byte[] key, iv;
            string fileName = @"C:\Users\\vicos\Desktop\work\workWithRoi\EncrypDecrypProject\KeyAndIv.json";
            EncryptDecrypt.ReadKeyAndIvFromFile(out key, out iv, ref fileName);
            byte[] encryptedMessage = EncrypMessage(message, key, iv);
            return await SendMessageToOtherServer(encryptedMessage);
        }
        private static async Task<string> SendMessageToOtherServer(byte[] encryptedMessage)
        {
            try
            {
                string newUrl = url + "decrypting-encrypted-data";
                var json = JsonConvert.SerializeObject(encryptedMessage);
                HttpContent dataToSend = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(newUrl, dataToSend);
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Data);
            }
            return "You Failled";
        }
        private static byte[] EncrypMessage(string message, byte[] key, byte[] iv)
        {
            byte[] encryptedMessage;
            using (Aes aes = Aes.Create())
            {
                ICryptoTransform cryptoEncryptor = aes.CreateEncryptor(key, iv);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoEncryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(message);
                        }
                        encryptedMessage = memoryStream.ToArray();
                    }
                }
            }
            return encryptedMessage;
        }
    }
}
