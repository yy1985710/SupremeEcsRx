using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcsRx.Unity;


namespace EcsRx.Network.Data
{
    public class HttpResponseData<T>
    {
        [JsonProperty] public int code { get; set; }
        [JsonProperty] public string msg { get; set; }
        [JsonProperty] public T data { get; set; }
    }
}
