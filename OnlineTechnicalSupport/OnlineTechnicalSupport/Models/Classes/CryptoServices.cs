using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace OnlineTechnicalSupport.Models.Classes
{
    public class CryptoServices
    {
        public string CreateCode(int length)
        {
            //Create tag for request
            char[] chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[8];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder code = new StringBuilder(length);
            foreach (byte b in data)
            {
                code.Append(chars[b % (chars.Length)]);
            }
            return code.ToString();
        }
    }
}