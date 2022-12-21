using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MtekAssetBundle
{
    public static Hash128 assetHash;
    public static AssetBundle _myLoadedAssetBundle;
    static uint crcR;
    public static IEnumerator DownloadAssetBundle(string url, string assetName, Action<int> progress, string _getDataSceneName)
    {
        //Get from generated manifest file of assetbundle.
        uint crcNumber = crcR;
        //Get from generated manifest file of assetbundle.
        Hash128 hashCode = assetHash;
        UnityWebRequest webrequest =
           UnityWebRequestAssetBundle.GetAssetBundle(url, new CachedAssetBundle(assetName, hashCode), crcNumber);


        webrequest.SendWebRequest();

        while (!webrequest.isDone)
        {
            Debug.Log(webrequest.downloadProgress);
        }

        _myLoadedAssetBundle = DownloadHandlerAssetBundle.GetContent(webrequest);
        yield return _myLoadedAssetBundle;
        if (_myLoadedAssetBundle == null)
            yield break;


        //Gets name of all the assets in that assetBundle.
        yield return LoadAssetBundle(progress, _getDataSceneName);
        _myLoadedAssetBundle.Unload(false);
        yield return null;
    }
    
    static LoadSceneMode loadSceneMode = LoadSceneMode.Single;

    public static IEnumerator LoadAssetBundle(Action<int> progress, string _getDataSceneName)
    {
        if (!_myLoadedAssetBundle)
        {
            Debug.LogError("Could not load null asset bundle");
            yield break;
        }
        string _sceneName;
        if (_myLoadedAssetBundle.isStreamedSceneAssetBundle)
        {
            Debug.Log("Streaming asset in");

            string[] scenePaths = _myLoadedAssetBundle.GetAllScenePaths();
 
            int _sceneid;
            string _dataSceneName = _getDataSceneName;
            string[] _folderName = _dataSceneName.Split("_");
            _sceneid = scenePaths.findIndex("Assets/Games/"+ _folderName[0] +"/Scenes/" + _dataSceneName + ".unity");

            _sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePaths[_sceneid]);

            int  downloadProgress = 0;
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneName, loadSceneMode);
            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                Debug.Log("Loading");
                downloadProgress = (int)(asyncLoad.progress * 100f);
                if (downloadProgress == 89)
                {
                    downloadProgress = 100;
                    asyncLoad.allowSceneActivation = true;
                }
                progress?.Invoke(downloadProgress);
                yield return null;
            }

            asyncLoad.completed += (AsyncOperation asyncOp) =>
            {
                progress?.Invoke(100);
            };
        }
        else
        {
            string[] assets = _myLoadedAssetBundle.GetAllAssetNames();

            Debug.Log("Getting all " + assets.Length + " assets in");

            Debug.LogError("Scene Asset Bundle should be streaming type. Prefab loading and instancing without a main scene is not supported by this function.");
        }

    }
}
public static class Extensions
{
    public static int findIndex<T>(this T[] array, T item)
    {
        try
        {
            return array
                .Select((element, index) => new KeyValuePair<T, int>(element, index))
                .First(x => x.Key.Equals(item)).Value;
        }
        catch (InvalidOperationException)
        {
            return -1;
        }
    }
}