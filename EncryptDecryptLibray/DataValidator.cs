using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EncryptDecryptLibray
{
    public static class DataValidator
    {
        public static bool IsValidData(string data)
        {
            return string.IsNullOrEmpty(data);
        }
        public static bool IsValidByteArr(byte [] arr)
        {
            return (arr != null && arr.Length > 0);
        }
    }
}
