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
            if (DataValidator.IsNotValidData(message))
            {
                throw new ArgumentException("message is null or empty");
            }
            byte[] key, iv;
            string fileName = @"C:\Users\\vicos\Desktop\work\workWithRoi\EncrypDecrypProject\KeyAndIv.json";
            EncryptDecrypt.ReadKeyAndIvFromFile(out key, out iv, ref fileName);
            byte[] encryptedMessage = EncryptMessage(message, key, iv);
            return await SendMessageToOtherServer(encryptedMessage);
        }
        private static async Task<string> SendMessageToOtherServer(byte[] encryptedMessage)
        {
            if(DataValidator.IsValidByteArr(encryptedMessage) == false) 
            {
                throw new Exception("encrypted message not valid");
            }
            try
            {
                string decryptingUrl = $"{url}decrypting-encrypted-data";
                var json = JsonConvert.SerializeObject(encryptedMessage);
                HttpContent dataToSend = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(decryptingUrl, dataToSend);
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Data);
            }
            return "You Failled";
        }
        private static byte[] EncryptMessage(string message, byte[] key, byte[] iv)
        {
            byte[] encryptedMessage = null;
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
