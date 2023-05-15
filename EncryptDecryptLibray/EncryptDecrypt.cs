using Newtonsoft.Json;
using System.Security.Cryptography;

namespace EncryptDecryptLibray
{
    public static class EncryptDecrypt
    {
        public static void CreateKeyAndVector(out byte[] key, out byte[] iv, string fileName)
        {
            CheckIfFileStringIsGood(fileName);
            key = new byte[16];
            iv = new byte[16];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
                rng.GetBytes(iv);
            }
            CreateKeyAndIvFile(ref key, ref iv, ref fileName);
        }
        private static void CreateKeyAndIvFile(ref byte[] key, ref byte[] iv, ref string fileName)
        {
            KeyAndIvObject keyAndIvObject = new KeyAndIvObject()
            {
                Key = key,
                IV = iv
            };
            string stringjson = JsonConvert.SerializeObject(keyAndIvObject);
            File.WriteAllText(fileName, stringjson);
        }
        public static void ReadKeyAndIvFromFile(out byte[] key, out byte[] iv, ref string fileName)
        {
            CheckIfFileStringIsGood(fileName);
            StreamReader r = new StreamReader(fileName);
            string json = r.ReadToEnd();
            KeyAndIvObject item = JsonConvert.DeserializeObject<KeyAndIvObject>(json);
            if(item == null)
            {
                throw new Exception("couldn't get key or iv");
            }
            key = item.Key;
            iv = item.IV;
        }
        private static void CheckIfFileStringIsGood(string fileName)
        {
            if (DataValidator.IsValidData(fileName))
            {
                throw new ArgumentException("fileName is null or empty");
            }
        }
    }
}