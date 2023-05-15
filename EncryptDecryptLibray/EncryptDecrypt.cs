using Newtonsoft.Json;
using System.Security.Cryptography;

namespace EncryptDecryptLibray
{
    public static class EncryptDecrypt
    {
        public static void CreateKeyAndVector(out byte[] key, out byte[] iv, string fileName)
        {
            key = new byte[16];
            iv = new byte[16];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
                rng.GetBytes(iv);
            }
            CrateKeyAndIvFile(ref key, ref iv, ref fileName);
        }
        private static void CrateKeyAndIvFile(ref byte[] key, ref byte[] iv, ref string fileName)
        {
            KeyAndIvObject keyAndIvObject = new KeyAndIvObject()
            {
                Key = key,
                IV = iv
            };
            string stringjson = JsonConvert.SerializeObject(keyAndIvObject);
            System.IO.File.WriteAllText(fileName, stringjson);
        }
        public static void ReadKeyAndIvFromFile(out byte[] key, out byte[] iv, ref string fileName)
        {
            StreamReader r = new StreamReader(fileName);
            string json = r.ReadToEnd();
            KeyAndIvObject item = JsonConvert.DeserializeObject<KeyAndIvObject>(json);
            key = item.Key;
            iv = item.IV;
        }
    }
}