using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.Loader
{
    public class LocalPriorLoadStrategy : ILoadStrategy
    {

        public string Folder { get; set; }
        public string Extension { get; set; }
        public Func<WWW, byte[]> FileEncode { get; set; }
        private LocalFileLoader localFileLoader;
        private RemoteFileLoader remoteFileLoader;

        public string Path
        {
            get
            {
                //pc,ios //android :jar:file//  
                return Application.persistentDataPath + Folder;

            }
        }

        public LocalPriorLoadStrategy(LocalFileLoader localFileLoader, RemoteFileLoader remoteFileLoade)
        {
            this.localFileLoader = localFileLoader;
            this.remoteFileLoader = remoteFileLoade;
            Folder = "/Cache/";
        }

        public IObservable<WWW> Load(string url)
        {
            var hashCodeUrl = Path + url.GetHashCode();
            //如果之前不存在缓存文件
            if (!File.Exists(hashCodeUrl))
            {
                var www = remoteFileLoader.Load(url);
                www.Subscribe(file =>
                {
                    if (file != null)
                    {
                        var encodeBytes = FileEncode?.Invoke(file);
                        File.WriteAllBytes(hashCodeUrl, encodeBytes);
                    }
                } );
               
                return www;
            }
            else
            {
                return localFileLoader.Load(hashCodeUrl);
            }
        }
    }
}
