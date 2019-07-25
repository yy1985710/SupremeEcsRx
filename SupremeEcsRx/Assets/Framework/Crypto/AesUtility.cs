using System;
using System.Security.Cryptography;
using System.IO;

namespace EcsRx.Crypto
{
    public class AesUtility
    {
        public static byte[] Encrption(byte[] input, string key, string iv)
        {
            byte[] keys = System.Text.Encoding.UTF8.GetBytes(key);
            byte[] ivs = System.Text.Encoding.UTF8.GetBytes(iv);
            RijndaelManaged aes = new RijndaelManaged();

            aes.Key = keys;
            aes.IV = ivs;

            ICryptoTransform transform = aes.CreateEncryptor();
            byte[] resultArray = transform.TransformFinalBlock(input, 0, input.Length);
            return resultArray;
        }

        public static byte[] Decrption(byte[] input, string key, string iv)
        {
            byte[] keys = System.Text.Encoding.UTF8.GetBytes(key);
            byte[] ivs = System.Text.Encoding.UTF8.GetBytes(iv);
            RijndaelManaged aes = new RijndaelManaged();

            aes.Key = keys;
            aes.IV = ivs;

            ICryptoTransform transform = aes.CreateDecryptor();
            byte[] resultArray = transform.TransformFinalBlock(input, 0, input.Length);
            return resultArray;
        }
    }
}
