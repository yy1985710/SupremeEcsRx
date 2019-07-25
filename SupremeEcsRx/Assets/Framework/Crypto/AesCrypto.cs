using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcsRx.Crypto
{
    public class AesCrypto : ICrypto
    {
        public string Key { get; set; }
        public string InitialVector { get; set; }

        public AesCrypto(string key, string iv)
        {
            Key = key;
            InitialVector = iv;
        }

        public byte[] Encryption(byte[] data)
        {
            return AesUtility.Encrption(data, Key, InitialVector);
        }

        public byte[] Decryption(byte[] data)
        {
            return AesUtility.Decrption(data, Key, InitialVector);
        }
    }
}
