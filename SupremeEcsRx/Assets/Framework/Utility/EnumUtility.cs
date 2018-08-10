using System;
using UnityEngine;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace EcsRx.Utility
{

    public static class EnumUtility
    {
        public static string GetDescription(this Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute) attrs[0]).Description;
                }
            }
            return en.ToString();
        }
    }
}

