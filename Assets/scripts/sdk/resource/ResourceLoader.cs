using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ResourceLoader  
{
    //public static ResourceLoader instance=new ResourceLoader();
    //下面3个状态适用于所有协程
    public const int STATE_LOADING=1;
    public const int STATE_NOT_BEGIN=0;
    public const int STATE_DONE=2;
    public static string ResourceVariant = "md";
    public static string streamingAssetsPathURL;
    public class AssetPathAndName
    {
        public string path;
        public string name;
    }
    public class LoadedAssetBundle
    {
        public AssetBundle assetBundle;
        public int state;
    }
    public class LoadedAsset
    {
        public object asset;
        public int state;
    }
    //存储资源依赖信息
    private static Dictionary<string, List<AssetPathAndName>> resources = new Dictionary<string, List<AssetPathAndName>>();
    //已经加载到内存的资源
    private static Dictionary<string, LoadedAsset> loadedAssets=new Dictionary<string, LoadedAsset>();
    //已经加载到内存的资源包
    private static Dictionary<string, LoadedAssetBundle> loadedAssetBundles = new Dictionary<string, LoadedAssetBundle>();
    //public Dictionary<string, LoadedAssetBundle> loadedAssetBundles=new Dictionary<string, LoadedAssetBundle>();

    //static string replaceSlashes(string s)
    //{
    //    if (s.Contains("\\"))
    //    {
    //        s=s.Replace("\\", "|");
    //    }
    //    if (s.Contains("/"))
    //    {
    //        s=s.Replace("/", "|");
    //    }
    //    return s;
    //}
    public static void init()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                {
                    streamingAssetsPathURL = "jar:file://" + Application.dataPath + "!/assets/";
                }
                break;
            case RuntimePlatform.WindowsEditor:
                {
                    streamingAssetsPathURL = "file://" + Application.dataPath + "/StreamingAssets/";
                }
                break;
            case RuntimePlatform.IPhonePlayer:
                {
                    streamingAssetsPathURL = Application.dataPath + "/Raw/";
                }
                break;
            case RuntimePlatform.WindowsPlayer:
                {
                    streamingAssetsPathURL = "file://" + Application.dataPath + "/StreamingAssets/";
                }
                break;
        }
        TextAsset ta=Resources.Load("needNotPackage/assetsInfo") as TextAsset;
        CSVReader csvReader = new CSVReader();
        csvReader.load(ta.bytes);
        while (csvReader.readRecord())
        {
            string key =  csvReader.get(0)   + "|" + csvReader.get(1) ;
            resources[key] = new List<AssetPathAndName>();
           
            for (int i = 2; i < csvReader.getColumnCount() - 1; i++)
            {
                //Debug.Log(csvReader.get(i));
                string[] depPathAndName = csvReader.get(i).Split('|');
                AssetPathAndName assetPathAndName = new AssetPathAndName();
                assetPathAndName.path = depPathAndName[0];
                assetPathAndName.name = depPathAndName[1];
                resources[key].Add(assetPathAndName);
            }
        }
    }
    void Update()
    {
    }
    public static void LoadFromResourcesDir(ResourceLoadTask task)
    {
        task.asset = Resources.Load(task.path + "/" + task.name.Split('.')[0]);
        task.state = STATE_DONE;
    }
    public static IEnumerator loadFromResourcesDirAsync(ResourceLoadTask task)
    {
        //加载resources文件夹里的文件，不能有后缀。
        ResourceRequest resourceRequest = Resources.LoadAsync(task.path + "/" + task.name.Split('.')[0]);
        while (!resourceRequest.isDone)
        {
            yield return true;
        }
        task.asset = resourceRequest.asset;
        task.state = STATE_DONE;
    }
    private static IEnumerator loadGroupFromResourcesDirAsync(ResourceLoadTaskGroup group)
    {
        group.state = STATE_LOADING;
        foreach (var task in group.getTaskList())
        {
            yield return loadFromResourcesDirAsync(task);
            ////加载resources文件夹里的文件，不能有后缀。
            //ResourceRequest resourceRequest = Resources.LoadAsync(task.path + "/" + task.name.Split('.')[0]);
            //while (!resourceRequest.isDone)
            //{
            //    yield return true;
            //}
            //task.asset = resourceRequest.asset;
            //task.state = STATE_DONE;
            group.progress++;
        }
        group.state = STATE_DONE;
    }
    //private static IEnumerator loadFromAssetaBundleAsync(ResourceLoadTask task)
    //{
    //    task.name = task.name.Trim().ToLower();
    //    task.path = task.path.Trim().ToLower();
    //    Dictionary<string, LoadedAssetBundle> loadedAssetBundles = new Dictionary<string, LoadedAssetBundle>();
    //    string key =  task.path + "|" + task.name.ToLower();
        
    //    if (!resources.ContainsKey(key))
    //    {
    //        Debug.LogError("*****" + key);
    //        foreach (var VARIABLE in resources.Keys)
    //        {
    //            Debug.LogError(VARIABLE);
    //        }
    //    }
        
    //    //先加载依赖资源
    //    foreach (AssetPathAndName assetPathAndName in resources[key])
    //    {
    //        yield return loadFromAssetaBundle(assetPathAndName.path, assetPathAndName.name, loadedAssetBundles);
    //    }
    //    //加载资源
    //    if (loadedAssetBundles.ContainsKey(task.path))
    //    {
    //        task.resource = loadedAssetBundles[task.path].assetBundle.LoadAsset(task.name);
    //    }
    //    else
    //    {
    //        WWW asset = new WWW(streamingAssetsPathURL   + task.path + "." + ResourceVariant);
    //        yield return asset;
    //        task.resource =  asset.assetBundle.LoadAsset(task.name.ToLower());
    //        asset.assetBundle.Unload(false);
    //    }
    //    //AssetBundleCreateRequest request= AssetBundle.LoadFromFileAsync(Application.dataPath + "/StreamingAssets/" + task.path + "." + ResourceVariant);
    //    //while (!request.isDone)
    //    //{
    //    //    yield return true;
    //    //}
    //    //AssetBundle bundle = request.assetBundle;
    //    foreach (LoadedAssetBundle loadedAssetBundle in loadedAssetBundles.Values)
    //    {
    //        loadedAssetBundle.assetBundle.Unload(false);
    //    }
    //    task.finished = true;
    //}

    //private string getKey(ResourceLoadTask task)
    //{
    //    task.name = task.name.Trim().ToLower();
    //    task.path = task.path.Trim().ToLower();
    //    string key = task.path + "|" + task.name;
    //}
    private static IEnumerator loadGroupFromAssetBundleAsync(ResourceLoadTaskGroup group)
    {
        group.state = STATE_LOADING;
        foreach (var task in group.getTaskList())
        {
            yield return loadFromAssetaBundleAsync(task);
            group.progress++;
        }
        //foreach (LoadedAssetBundle loadedAssetBundle in loadedAssetBundles.Values)
        //{
        //    loadedAssetBundle.assetBundle.Unload(false);
        //}
        group.state = ResourceLoader.STATE_DONE;
    }
    private static IEnumerator loadFromAssetaBundleAsync(ResourceLoadTask task )
    {
        task.state = STATE_LOADING;
        task.name = task.name.Trim().ToLower();
        task.path = task.path.Trim().ToLower();
        string key = task.path + "|" + task.name;
        LoadedAsset loadedAsset = null;
        //如果资源已经在其他地方加载了，等待加载完成，然后直接获取
        if (loadedAssets.ContainsKey(key))
        {
            loadedAsset = loadedAssets[key];
            while (loadedAsset.state != STATE_DONE)
            {
                yield return 0;
            }
            task.state = STATE_DONE;
            task.asset = loadedAsset.asset;
            yield break;
        }
        else
        {
            loadedAsset = new LoadedAsset();
            loadedAsset.state = STATE_LOADING;
            loadedAssets[key] = loadedAsset;
        }
        //先加载依赖资源
        if (!resources.ContainsKey(key))
        {
            Debug.LogError(key);
            foreach (var VARIABLE in resources.Keys)
            {
                Debug.LogError(VARIABLE);
            }
            task.asset = null;
            task.state = STATE_DONE;
            yield break;
        }
        foreach (AssetPathAndName assetPathAndName in resources[key])
        {
            ResourceLoadTask t = new ResourceLoadTask();
            t.name = assetPathAndName.name;
            t.path = assetPathAndName.path;
            yield return loadFromAssetaBundleAsync( t);
            // Debug.Log(assetPathAndName.path + "  " + assetPathAndName.name);
        }
        LoadedAssetBundle bundle = null;
        if (loadedAssetBundles.ContainsKey(task.path))
        {
            bundle = loadedAssetBundles[task.path];
            if (bundle.state!=STATE_DONE)
            {
                yield return 0;
            }
        }
        else
        {
            bundle = new LoadedAssetBundle();
            loadedAssetBundles[task.path] = bundle;
            bundle.state = STATE_LOADING;
            WWW asset = new WWW(streamingAssetsPathURL   + task.path + "." + ResourceVariant);
            yield return asset;
            bundle.assetBundle = asset.assetBundle;
            bundle.state = STATE_DONE;
        }
        loadedAsset.asset =bundle.assetBundle.LoadAsset(task.name);
        loadedAsset.state = STATE_DONE;
        task.state = STATE_DONE;
        task.asset = loadedAsset.asset;
    }
    public static IEnumerator LoadAssetAsync(ResourceLoadTask task)
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                {
                    yield return loadFromAssetaBundleAsync(task);
                }
                break;
            case RuntimePlatform.WindowsEditor:
                {
                    //yield return loadFromAssetaBundleAsync(task);
                    yield return   loadFromResourcesDirAsync(task) ;
                }
                break;
        }
        yield return  loadFromResourcesDirAsync(task);
    }
    //public Coroutine LoadAssetAsync(ResourceLoadTask task)
    //{
    //    switch (Application.platform)
    //    {
    //        case RuntimePlatform.Android:
    //            {
    //                return StartCoroutine(loadFromAssetaBundleAsync(task));
    //            }
    //            break;
    //        case RuntimePlatform.WindowsEditor:
    //            {
    //                //return StartCoroutine(loadFromAssetaBundle(task));
    //                return StartCoroutine(loadFromResourcesDirAsync(task));
    //            }
    //            break;
    //    }
    //      return StartCoroutine(loadFromResourcesDirAsync(task));
    //}
    public static IEnumerator LoadGroupAsync(ResourceLoadTaskGroup group )
    {
        switch (Global.PlatformType)
        {
            case Global.UNITY_ANDROID:
                {
                   yield return loadGroupFromAssetBundleAsync (group) ;
                }
                break;
            case Global.UNITY_EDITOR:
                {
                    yield return loadGroupFromResourcesDirAsync(group) ;
                }
                break;
        }
        yield return loadGroupFromResourcesDirAsync(group) ;
    }
   
}
