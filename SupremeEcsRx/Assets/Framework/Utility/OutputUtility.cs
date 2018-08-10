using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using ProtoBuf;

namespace EcsRx.Utility
{
    public static class OutputUtility
    {
        public static string PrintToString(object data)
        {
            StringBuilder output = new StringBuilder();
            var type = data.GetType();
            if (type.IsDefined(typeof(ProtoContractAttribute), true))
            {
                foreach (var propertyInfo in type.GetProperties())
                {
                    if (propertyInfo.IsDefined(typeof(ProtoMemberAttribute), true))
                    {
                        StringBuilder value = new StringBuilder();
                        if (typeof(ICollection).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            value.Append("{");
                            var array = propertyInfo.GetValue(data) as IEnumerable;
                            int index = 0;
                            if (array != null)
                                foreach (var v in array)
                                {
                                    var arrayType = v.GetType();
                                    if (arrayType.IsDefined(typeof(ProtoContractAttribute), true))
                                    {
                                        value.Append($"{index}={{{PrintToString(v)}}}, ");
                                    }
                                    else
                                    {
                                        value.Append($"{index}={v}, ");
                                    }
                                    index++;
                                }
                            value.Append("}");
                        }
                        else
                        {
                            var propertyValue = propertyInfo.GetValue(data);
                            if (propertyValue != null && !propertyInfo.PropertyType.IsEnum && propertyValue.GetType().IsDefined(typeof(ProtoContractAttribute), true))
                            {
                                value.Append("{");
                                value.Append(PrintToString(propertyInfo.GetValue(data)));
                                value.Append("}");
                            }
                            else
                            {
                                value.Append(propertyInfo.GetValue(data));
                            }
                            
                        }
                        
                        output.AppendFormat("{0} = {1}, ", propertyInfo.Name, value);
                    }
                }
            }
            return output.ToString();
        }
        
    }

}

