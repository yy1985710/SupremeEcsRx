﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcsRx.Crypto
{
    public interface ICrypto
    {
        byte[] Encryption(byte[] data);
        byte[] Decryption(byte[] data);
    }
}
