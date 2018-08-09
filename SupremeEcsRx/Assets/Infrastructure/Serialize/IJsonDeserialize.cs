﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EcsRx.Unity;

namespace EcsRx.Serialize
{
    public interface IJsonDeserialize : IDeserialize
    {
        object Deserialize(System.Type type, JSONNode data);
    }
}
