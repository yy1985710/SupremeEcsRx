using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;

namespace EcsRx.Unity.Loader
{
    public class ImageLoader : Loader<Texture2D>
    {

        public ImageLoader(ILoadStrategy loadStrategy, LocalFileLoader localFileLoader) : base(loadStrategy, localFileLoader)
        {
            loadStrategy.Folder = "/ImageCache/";
            loadStrategy.FileEncode = www => www.texture.EncodeToPNG();
            if (!Directory.Exists(loadStrategy.Path))
            {
                Directory.CreateDirectory(loadStrategy.Path);
            }
        }

        public override IObservable<Texture2D> LoadFromLocal(string url)
        {
            var www = localFileLoader.Load(url);
            return www.Select(www1 =>
            {
                var texture = www1?.texture;
                www1?.Dispose();
                return texture;
            });
        }

        public override IObservable<Texture2D> LoadFromRemote(string url)
        {
            var www = loadStrategy.Load(url);
            return www.Select(www1 =>
            {
                var texture = www1?.texture;
                www1?.Dispose();
                return texture;
            });
        }
    }
}
