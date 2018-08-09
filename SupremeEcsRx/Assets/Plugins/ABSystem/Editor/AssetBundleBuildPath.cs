using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace NO1Software.ABSystem.Editor
{
    public static class AssetBundleBuildPath
    {
        /// <summary>
        /// AB 保存的路径
        /// </summary>
        public static string BundleSavePath { get { return $"BuildData/{EditorUserBuildSettings.activeBuildTarget}/{AssetBundlePathResolver.Instance.BundleSaveDirName}"; } }
        /// <summary>
        /// AB打包的原文件HashCode要保存到的路径，下次可供增量打包
        /// </summary>
        public static string HashCacheSaveFile { get { return $"{BundleSavePath}/cache.txt"; } }

    }
}
