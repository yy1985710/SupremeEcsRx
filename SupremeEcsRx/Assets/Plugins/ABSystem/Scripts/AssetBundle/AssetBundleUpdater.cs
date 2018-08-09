using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text;
using NO1Software.Plugins.util;
using UniRx;
using UnityEngine;

namespace NO1Software.ABSystem
{ 
    public class AssetBundleUpdater
    {
        //The list stored AssetBundle names which will be update  
        private List<string> m_NeedDownloadAssetBundleNamesList = new List<string>();
        //if occur error when update Assetbundle from server
        private bool m_HasError;

        private bool deleteFolder;
        public event Action AssetBundleUpdateCompleteEvent;
        public event Action<float> AssetBundleUpdateProcessEvent;
        public event Action<bool> AssetBundleUpdateErrorEvent;
        /// <summary>
        /// start update AssetBundles
        /// </summary>
        public void StartUpdateAssetBundles()
        {
            MainThreadDispatcher.StartCoroutine(AssetBundlesUpdateStart());
        }

        IEnumerator AssetBundlesUpdateStart()
        {
            string assetBundleServerPath = AssetBundlePathResolver.Instance.AssetBundleServerPath;
            string assetBundleLocalPath = AssetBundlePathResolver.Instance.BundleCacheDir;

            yield return MainThreadDispatcher.StartCoroutine(UpdateAssetBundle(assetBundleServerPath, assetBundleLocalPath));
            if (!m_HasError)
            {
                AssetBundleUpdateCompleteEvent?.Invoke();
            }
            else
            {
                AssetBundleUpdateErrorEvent?.Invoke(deleteFolder);
            }
        }

        IEnumerator UpdateAssetBundle(string assetBundleServerPath, string assetBundleLocalPath)
        {
            string localAssetBundleManifestPath = $"{assetBundleLocalPath}/{AssetBundlePathResolver.Instance.BundleSaveDirName}";
            string serverAssetBundleManifestPath = $"{assetBundleServerPath}/{AssetBundlePathResolver.Instance.BundleSaveDirName}";

            string localDependFilePath = $"{assetBundleLocalPath}/{AssetBundlePathResolver.Instance.DependFileName}";
            string serverDependFilePath = $"{assetBundleServerPath}/{AssetBundlePathResolver.Instance.DependFileName}";

            AssetBundleManifest localAssetBundleManifest = null;
            AssetBundleManifest serverAssetBundleManifest = null;
            if (FileUtility.CheckFileIsExist(localAssetBundleManifestPath) == false)
            {
                yield return MainThreadDispatcher.StartCoroutine(DownLoadAssetBundle(serverAssetBundleManifestPath, localAssetBundleManifestPath));
                if (m_HasError)
                {
                    deleteFolder = true;
                    yield break;
                }

                yield return MainThreadDispatcher.StartCoroutine(DownLoadAssetBundle(serverDependFilePath, localDependFilePath));
                if (m_HasError)
                {
                    deleteFolder = true;
                    yield break;
                }

                localAssetBundleManifest = LoadLocalAssetBundleManifest(localAssetBundleManifestPath);
                m_NeedDownloadAssetBundleNamesList = localAssetBundleManifest.GetAllAssetBundles().ToList();
                int currentCount = 0;
                for (int i = 0; i < m_NeedDownloadAssetBundleNamesList.Count; i++)
                {
                    string assetBundleName = m_NeedDownloadAssetBundleNamesList[i];

                    string serverAssetBundlesPath = string.Format("{0}/{1}", assetBundleServerPath, assetBundleName);

                    string localAssetBundlesPath = string.Format("{0}/{1}", assetBundleLocalPath, assetBundleName);

                    yield return MainThreadDispatcher.StartCoroutine(DownLoadAssetBundle(serverAssetBundlesPath, localAssetBundlesPath));

                    if (m_HasError)
                    {
                        deleteFolder = true;
                        yield break;
                    }

                    currentCount++;
                    AssetBundleUpdateProcessEvent?.Invoke((float)currentCount/m_NeedDownloadAssetBundleNamesList.Count);
                }
            }
            else
            {

                byte[] localAssetBundleManifestBytes = FileUtility.FileConvertToBytes(localAssetBundleManifestPath);
                //load AssetBundleManifest to memory from server
                WWW serverWWW = new WWW(serverAssetBundleManifestPath);

                yield return serverWWW;

                if (serverWWW.error != null)
                {
                    m_HasError = true;
                    yield break;
                }

                byte[] serverAssetBundleManifestBytes = serverWWW.bytes;
                serverWWW.Dispose();
                
                if (HashUtil.Get(localAssetBundleManifestBytes) == HashUtil.Get(serverAssetBundleManifestBytes))
                {
                    AssetBundleUpdateProcessEvent?.Invoke(1);
                    yield break;
                }

                yield return MainThreadDispatcher.StartCoroutine(DownLoadAssetBundle(serverDependFilePath, localDependFilePath));
                if (m_HasError)
                {
                    deleteFolder = true;
                    yield break;
                }

                serverAssetBundleManifest = LoadAssetBundleManifest(serverAssetBundleManifestBytes);
                localAssetBundleManifest = LoadLocalAssetBundleManifest(localAssetBundleManifestPath);

                string[] localAssetBundleNames = localAssetBundleManifest.GetAllAssetBundles();
                string[] serverAssetBundleNames = serverAssetBundleManifest.GetAllAssetBundles();

                ///Check each AssetBundles from server 
                for (int i = 0; i < serverAssetBundleNames.Length; i++)
                {
                    string assetBundleName = serverAssetBundleNames[i];

                    if (localAssetBundleNames.Contains(assetBundleName))
                    {
                        Hash128 localAssetBundleHash = localAssetBundleManifest.GetAssetBundleHash(assetBundleName);

                        Hash128 serverAssetBundleHash = serverAssetBundleManifest.GetAssetBundleHash(assetBundleName);

                        //if the hash value is different,delete the AssetBundle file in local machine,then download the AssetBundle from server  
                        if (localAssetBundleHash != serverAssetBundleHash)
                        {
                            m_NeedDownloadAssetBundleNamesList.Add(assetBundleName);
                        }
                    }
                    else
                    {
                        m_NeedDownloadAssetBundleNamesList.Add(assetBundleName);
                    }
                }

                //delete redundant AssetBundles in local machine
                for (int i = 0; i < localAssetBundleNames.Length; i++)
                {
                    string assetBundleName = localAssetBundleNames[i];

                    if (!serverAssetBundleNames.Contains(assetBundleName))
                    {
                        string deleteAssetBundlePath = string.Format("{0}/{1}", assetBundleLocalPath, assetBundleName);

                        //delete redundant AssetBundles
                        FileUtility.DeleteFile(deleteAssetBundlePath);
                        FileUtility.IfDeletedFileCurrentDirectoryIsEmptyDeleteRecursively(deleteAssetBundlePath);
                    }
                }
                int currentCount = 0;
                for (int i = 0; i < m_NeedDownloadAssetBundleNamesList.Count; i++)
                {
                    string assetBundleName = m_NeedDownloadAssetBundleNamesList[i];

                    string serverAssetBundlesPath = string.Format("{0}/{1}", assetBundleServerPath, assetBundleName);

                    string localAssetBundlesPath = string.Format("{0}/{1}", assetBundleLocalPath, assetBundleName);

                    yield return MainThreadDispatcher.StartCoroutine(DownLoadAssetBundle(serverAssetBundlesPath, localAssetBundlesPath));

                    if (m_HasError)
                    {
                        deleteFolder = true;
                        yield break;
                    }

                    currentCount++;
                    AssetBundleUpdateProcessEvent?.Invoke((float)currentCount / m_NeedDownloadAssetBundleNamesList.Count);
                }

                FileUtility.BytesConvertToFile(serverAssetBundleManifestBytes, localAssetBundleManifestPath);
            }
            AssetBundleUpdateProcessEvent?.Invoke(1);
            serverAssetBundleManifest = null;
            localAssetBundleManifest = null;
            Resources.UnloadUnusedAssets();
        }

        private AssetBundleManifest LoadLocalAssetBundleManifest(string path)
        {
            byte[]  localAssetBundleManifestBytes = FileUtility.FileConvertToBytes(path);
            return LoadAssetBundleManifest(localAssetBundleManifestBytes);
        }

        private AssetBundleManifest LoadAssetBundleManifest(byte[] bytes)
        {
            AssetBundle assetbundle = AssetBundle.LoadFromMemory(bytes);
            AssetBundleManifest assetBundleManifest = (AssetBundleManifest)assetbundle.LoadAsset("AssetBundleManifest");
            assetbundle.Unload(false);
            return assetBundleManifest;
        }

        private IEnumerator DownLoadAssetBundles(List<string> assetBundles, string assetBundleServerPath,
            string assetBundleLocalPath)
        {
            int currentCount = 0;
            for (int i = 0; i < assetBundles.Count; i++)
            {
                string assetBundleName = assetBundles[i];

                string serverAssetBundlesPath = string.Format("{0}/{1}", assetBundleServerPath, assetBundleName);

                string localAssetBundlesPath = string.Format("{0}/{1}", assetBundleLocalPath, assetBundleName);

                yield return MainThreadDispatcher.StartCoroutine(DownLoadAssetBundle(serverAssetBundlesPath, localAssetBundlesPath));

                if (m_HasError)
                {
                    deleteFolder = true;
                    yield break;
                }

                currentCount++;
            }
        }

        /// <summary>
        /// download AssetBundle from server to local machine 
        /// </summary>
        /// <param name="assetBundleServerPath"></param>
        /// <param name="assetBundleLocalPath"></param>
        /// <returns></returns>
        private IEnumerator DownLoadAssetBundle(string assetBundleServerPath, string assetBundleLocalPath)
        {
            WWW www = new WWW(assetBundleServerPath);

            yield return www;

            if (www.error != null)
            {
                Debug.LogErrorFormat("{0} download error : {1}", assetBundleServerPath, www.error.ToString());

                m_HasError = true;
            }

            byte[] bytes = www.bytes;

            www.Dispose();
            FileUtility.BytesConvertToFileIfFileCurrentDirectoryNotExistThenCreateDirectoryFirst(bytes, assetBundleLocalPath);
        }
       
    }
}
