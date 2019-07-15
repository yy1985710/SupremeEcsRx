using System;
using UnityEngine;
using System.Collections;
using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Plugins.Views.Components;
using EcsRx.Unity.Components;
using Zenject;

namespace EcsRx.Unity.Audio
{

    public class AudioManager : IInitializable
    {
        public static string AssetBundlePath = "Assets.AssetBundleResources.Sounds.";
        public static GameObject BackgroundMusic;
        public static GameObject BackgroundSound;
        private IEntityCollection defaultCollection;

        public AudioManager(IEntityCollectionManager collectionManager)
        {
            defaultCollection = collectionManager.GetCollection();
        }

        public void Initialize()
        {
            if (PlayerPrefs.HasKey("SoundVolume"))
            {
                AudioListener.volume = PlayerPrefs.GetFloat("SoundVolume");
            }
            if (PlayerPrefs.HasKey("Mute"))
            {
                var b = Convert.ToBoolean(PlayerPrefs.GetInt("Mute"));
                AudioListener.volume = b ? 0f : AudioListener.volume;
            }
        }

        public IEntity PlayBackgroundMusic(string name)
        {
            var entity = defaultCollection.CreateEntity();
            entity.AddComponents(new MusicComponent(), new RawResourceComponent { Name = name });
            return entity;
        }

        public IEntity PlayBackgroundSound(string name)
        {
            var entity = defaultCollection.CreateEntity();
            entity.AddComponent<ViewComponent>();
            entity.AddComponents(new SoundComponent { Name = name, Mount = BackgroundSound});
            return entity;
        }

		public void StopPlayBackGroundSound()
		{
			PlayerPrefs.SetFloat("SoundVolume", AudioListener.volume);
			AudioListener.volume = 0;
		}

		public void StartPlayBackGroundSound()
		{
			if (PlayerPrefs.HasKey("SoundVolume"))
			{
				AudioListener.volume = PlayerPrefs.GetFloat("SoundVolume");
			}
		}

	}
}

