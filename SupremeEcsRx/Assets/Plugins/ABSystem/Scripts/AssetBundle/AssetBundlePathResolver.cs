using System.IO;
using UnityEngine;
namespace NO1Software.ABSystem
{
    /// <summary>
    /// AB 打包及运行时路径解决器
    /// </summary>
    public class AssetBundlePathResolver
    {
        public static AssetBundlePathResolver instance;
        public bool IsRemote = false;
        public string ServerUrl = "http://localhost:8080";

        public string CurrentPlatform
        {
            get
            {
                string platform = Application.platform.ToString();
#if UNITY_EDITOR
                if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
                {
                    platform = $"{UnityEditor.EditorUserBuildSettings.activeBuildTarget}";
                }
#else
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    platform = "StandaloneWindows64";
                }
                else if (Application.platform == RuntimePlatform.OSXPlayer)
                {
                    platform = "StandaloneOSXUniversal";
                }
#endif
                else if (Application.platform == RuntimePlatform.Android)
                {
                    platform = "Android";
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    platform = "iOS";
                }

                return platform;
            }
        }
#if UNITY_EDITOR
        public string AssetBundleServerPath => IsRemote ? $"{ServerUrl}/Asset/{CurrentPlatform}/{BundleSaveDirName}" : $"file://{AsstetBundleOutputPath}";
#elif UNITY_ANDROID
            public string AssetBundleServerPath => IsRemote ? $"{ServerUrl}/Asset/{CurrentPlatform}/{BundleSaveDirName}" : $"{Application.streamingAssetsPath}/{BundleSaveDirName}";
#elif UNITY_IOS
        public string AssetBundleServerPath => IsRemote ? $"{ServerUrl}/Asset/{CurrentPlatform}/{BundleSaveDirName}" : $"file://{Application.streamingAssetsPath}/{BundleSaveDirName}";
#elif UNITY_STANDALONE_WIN
        public string AssetBundleServerPath => IsRemote ? $"{ServerUrl}/Asset/{CurrentPlatform}/{BundleSaveDirName}" : $"file://{Application.streamingAssetsPath}/{BundleSaveDirName}";
#else
        public string AssetBundleServerPath => IsRemote ? $"{ServerUrl}/Asset/{CurrentPlatform}/{BundleSaveDirName}" : $"file://{AsstetBundleOutputPath}";
#endif
        public string AsstetBundleOutputPath => $"{Application.dataPath}/../BuildData/{CurrentPlatform}/{BundleSaveDirName}";

        public static AssetBundlePathResolver Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AssetBundlePathResolver();
                }
                return instance;
            }
        }

        /// <summary>
        /// AB 保存的路径相对于 Assets/StreamingAssets 的名字
        /// </summary>
        public virtual string BundleSaveDirName { get { return "AssetBundles"; } }
        /// <summary>
        /// 在编辑器模型下将 abName 转为 Assets/... 路径
        /// 这样就可以不用打包直接用了
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        public string GetEditorModePath(string abName)
        {
            //将 Assets.AA.BB.prefab 转为 Assets/AA/BB.prefab
            abName = abName.Replace(".", "/");
            int last = abName.LastIndexOf("/");

            if (last == -1)
                return abName;

            string path = string.Format("{0}.{1}", abName.Substring(0, last), abName.Substring(last + 1));
            return path;
        }

        /// <summary>
        /// 获取 AB 源文件路径（打包进安装包的）
        /// </summary>
        /// <param name="path"></param>
        /// <param name="forWWW"></param>
        /// <returns></returns>
        public virtual string GetBundleSourceFile(string path, bool forWWW = true)
        {
            
            string filePath = null;
#if UNITY_EDITOR
            if (forWWW)
                filePath = string.Format("file://{0}/StreamingAssets/{1}/{2}", Application.dataPath, BundleSaveDirName, path);
            else
                filePath = string.Format("{0}/StreamingAssets/{1}/{2}", Application.dataPath, BundleSaveDirName, path);
#elif UNITY_ANDROID
            if (forWWW)
                filePath = $"{Application.streamingAssetsPath}/{BundleSaveDirName}/{path}";
            else
                filePath = string.Format("{0}!assets/{1}/{2}", Application.dataPath, BundleSaveDirName, path);
#elif UNITY_IOS
            if (forWWW)
                filePath = string.Format("file://{0}/Raw/{1}/{2}", Application.dataPath, BundleSaveDirName, path);
            else
                filePath = string.Format("{0}/Raw/{1}/{2}", Application.dataPath, BundleSaveDirName, path);
#elif UNITY_STANDALONE_WIN
             if (forWWW)
                filePath = $"file://{Application.streamingAssetsPath}/{BundleSaveDirName}/{path}";
            else
            filePath = $"{Application.streamingAssetsPath}/{BundleSaveDirName}/{path}";
#else
            throw new System.NotImplementedException();
#endif
            return filePath;
        }

        /// <summary>
        /// AB 依赖信息文件名
        /// </summary>
        public virtual string DependFileName { get { return "dep.all"; } }

        DirectoryInfo cacheDir;

        /// <summary>
        /// 用于缓存AB的目录，要求可写
        /// </summary>
        public virtual string BundleCacheDir
        {
            get
            {
                if (cacheDir == null)
                {
//#if UNITY_EDITOR
//                    string dir = string.Format("{0}/{1}", Application.streamingAssetsPath, BundleSaveDirName);
//#else
					string dir = string.Format("{0}/{1}", Application.persistentDataPath, BundleSaveDirName);
//#endif
                    cacheDir = new DirectoryInfo(dir);
                    if (!cacheDir.Exists)
                        cacheDir.Create();
                }
                return cacheDir.FullName;
            }
        }
    }
}