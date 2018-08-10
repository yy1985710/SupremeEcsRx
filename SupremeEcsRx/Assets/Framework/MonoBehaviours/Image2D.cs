using System;
using UnityEngine;
using System.Collections;
using EcsRx.Unity.Loader;
using UniRx;
using UnityEngine.UI;
using Zenject;
namespace EcsRx.Unity
{
    public class Image2D : MonoBehaviour
    {
        public RawImage Texture;
        private ImageLoader imageLoader;
        private string path;
        private IDisposable disposable;
		public string Path
        {
            get { return path; }
            set
            {
                disposable?.Dispose();
                path = value;
				
				disposable = imageLoader.LoadFromRemote(path).CatchIgnore((WWWErrorException ex) =>
				{
				    Debug.Log(ex.RawErrorMessage);
                }).Subscribe(texture =>
				{
					Texture.texture = texture;
				});
			}
        }
		[Inject]
        public void Init(ImageLoader imageLoader)
        {
            this.imageLoader = imageLoader;
        }
    }
}

