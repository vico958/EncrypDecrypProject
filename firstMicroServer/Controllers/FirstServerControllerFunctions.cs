using Newtonsoft.Json;
using System.Text;
using EncryptDecryptLibray;
using System.Security.Cryptography;
using StackExchange.Redis;

namespace firstMicroServer.Controllers
{
    public static class FirstServerControllerFunctions
    {
        private static ConfigurationOptions redisConfig = ConfigurationOptions.Parse("localhost:6379");
        private static ConnectionMultiplexer redisConnection = ConnectionMultiplexer.Connect(redisConfig);
        private static IDatabase redisDatabase = redisConnection.GetDatabase();
        private static string url = "https://localhost:7170/";
        private static readonly HttpClient client = new HttpClient();
        public static async Task<string> EncrypMessageAsync(string message)
        {
            byte[] key, iv;
            string fileName = @"C:\Users\\vicos\Desktop\work\workWithRoi\EncrypDecrypProject\KeyAndIv.json";
            EncryptDecrypt.ReadKeyAndIvFromFile(out key, out iv, ref fileName);
            byte[] encryptedMessage;
            encryptedMessage = EncrypMessage(message, key, iv);
            try
            {
            redisDatabase.StringSet("myEncryptedMessage", encryptedMessage);
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return await SendMessageToOtherServer();
        }
        private static async Task<string> SendMessageToOtherServer()
        {
            var redisDatabase = redisConnection.GetDatabase();
            try
            {
                string newUrl = url + "decrypted-message-encrypted";
                var response = await client.GetAsync(newUrl);
                Console.WriteLine($"Decrypted message is : {redisDatabase.StringGet("myDcryptedMessage")}");
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Data);
            }
            return "You Failled";
        }
        /*        private static async Task<string> SendMessageToOtherServer(byte[] encryptedMessage)
                {
                    try
                    {
                        string newUrl = url + "decrypted-message-encrypted";
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
                }*/
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
