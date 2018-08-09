using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcsRx.Attributes;
using UnityEngine;

namespace EcsRx.Serialize
{
    public class WWWFormSerialize : IWWWFormSerialize
    {

        public byte[] Serialize(object data)
        {
            var properties = data.GetType().GetProperties();
            WWWForm form = new WWWForm();
            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.IsDefined(typeof(WWWFormProperty), true))
                {
                    var value = propertyInfo.GetValue(data, null);
                    if (value != null)
                    {
                        form.AddField(propertyInfo.Name, value.ToString());
                    }
                }
            }

            return form.data;
        }

        public byte[] Serialize<T>(T data)
        {
            return Serialize((object)data);
        }
    }
}
