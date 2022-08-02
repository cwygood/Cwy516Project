using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Common.Tools
{
    public class EncryptHelper
    {
        public static string MD5Encrypt(string content)
        {
            using(var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(content));
                return BitConverter.ToString(result).Replace("_", "");
            }
        }

        public static bool ValidateMd5(string strMd5, string str)
        {
            return strMd5 == MD5Encrypt(str);
        }
    }
}
