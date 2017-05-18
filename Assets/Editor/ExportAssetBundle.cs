using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AssetBundleUtil
{
    public static string assetBundleVariant = "md";
    public static string bundlePath= "Assets/StreamingAssets";
    public static string resourccePath = "Assets/Resources";
    [MenuItem("AssetBundle/test")]
    public static void test()
    {
        //CSVWriter csvWriter = new CSVWriter(bundlePath + "/test.csv");
        //foreach (string filePath in Directory.GetFiles("Assets/Resources/prefab/cubes/models"))
        //{

        //    if (filePath.EndsWith(".meta"))
        //    {
        //        continue;
        //    }
        //    string[] s = new string[1];
        //    s[0] = filePath;
        //    csvWriter.writeNext(s);
        //}
        //csvWriter.close();
    }


    [MenuItem("AssetBundle/Batch Set Asset Bundle Name (AssetBundleUtil)")]
    public static void BatchSetAssetBundleName()
    {
        foreach (string filePath in Directory.GetFiles("Assets/Resources"))
        {
            string lowerFilePath = filePath.ToLower();
            if (filePath.EndsWith(".meta"))
            {
                continue;
            }
            AssetImporter assetImporter = AssetImporter.GetAtPath(filePath);
            assetImporter.assetBundleName = "other";
            assetImporter.assetBundleVariant = assetBundleVariant;
        }
        //foreach (string directory in Directory.GetDirectories("Assets/Resources/"))
        //{
        //    Debug.Log(directory);
        //    setAssetBundleNameByDirectory(directory);
        //}
        CSVWriter csvWriter = new CSVWriter(resourccePath + "/needNotPackage/assetsInfo.csv");
        DirectoryInfo direction = new DirectoryInfo(resourccePath);
        DirectoryInfo[] allSubDirectorys = direction.GetDirectories("*", SearchOption.AllDirectories);
        foreach (DirectoryInfo dirInfo in allSubDirectorys)
        {
            //忽略needNotPackage文件夹
            if (dirInfo.Name.EndsWith("needNotPackage"))
            {
                continue;
            }
            string fullPath = dirInfo.FullName;
            string pathName = getPathRelativeWithPattern(fullPath, "Assets");
            
            string assetBundleName = getPathRelative(fullPath, "Resources");
            
            foreach (string filePath in Directory.GetFiles(pathName))
            {
                string lowerFilePath = filePath.ToLower();
                if (filePath.EndsWith(".meta"))
                {
                    continue;
                }
                
                AssetImporter assetImporter = AssetImporter.GetAtPath(lowerFilePath);
                assetImporter.assetBundleName = assetBundleName;
                assetImporter.assetBundleVariant = assetBundleVariant;
                //Debug.Log(getFileName(lowerFilePath));
                string[] depFilePaths = AssetDatabase.GetDependencies(filePath, false);
                List<string> strs=new List<string>();
                strs.Add(assetImporter.assetBundleName);
                strs.Add(getFileName(filePath).ToLower());
                 
                foreach (string depFilePath in depFilePaths)
                {
                    if (depFilePath.EndsWith(".cs"))
                    {
                        continue;
                    }
                    //Debug.Log(filePath + "     " + depFilePath);
                    //Debug.Log("      " + getFileDirectoryName(filePath));
                    string bundleName=getPathRelative(getFileDirectoryName(depFilePath), "Resources");
                    string fileName= getFileName(depFilePath);
                    strs.Add(bundleName.ToLower().Replace("\\","/") + "|" + fileName.ToLower());
                }
                csvWriter.writeNext(strs);
            }
        }
        csvWriter.close();
        Debug.Log("********************************Batch Set Asset Bundle Name finished");
    }

    public static string getFileDirectoryName(string name)
    {
        FileInfo fileInfo = new FileInfo(name);
        return fileInfo.DirectoryName;
    }
    public static string getFileName(string name)
    {
        FileInfo fileInfo = new FileInfo(name);
        return fileInfo.Name;
    }
    //public static string getFileNameWithOutExtension(string name)
    //{
    //    getFileName(name).Split();
    //     FileInfo fileInfo=new FileInfo(name);
    //    return fileInfo.Name;
    //}
    [MenuItem("AssetBundle/Export Asset Bundles (AssetBundleUtil)")]
    public static void ExportAssetBundles()
    {
        BuildAssetBundleOptions options = BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.DeterministicAssetBundle;
        // BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath + "/", options, EditorUserBuildSettings.activeBuildTarget);
        AssetBundleManifest assetBundleManifest = BuildPipeline.BuildAssetBundles(bundlePath, options, BuildTarget.Android);
        Debug.Log("********************************Build asset bundles finished");

        if (assetBundleManifest.GetAllAssetBundles() != null)
        {
            CSVWriter csvWriter = new CSVWriter(resourccePath + "/needNotPackage/assetBundlesInfo.csv");
            foreach (string asset in assetBundleManifest.GetAllAssetBundles())
            {
                //Debug.Log(asset + " ");
                foreach (string d in assetBundleManifest.GetAllDependencies(asset))
                {
                    //Debug.Log("    " + d);
                }
                string[] strs = { asset, assetBundleManifest.GetAssetBundleHash(asset).ToString() };
                csvWriter.writeNext(strs);
            }
            csvWriter.close();
        }

    }

    [MenuItem("AssetBundle/Reset All Asset Bundle Names (AssetBundleUtil)")]
    public static void ResetAssetBundleNames()
    {
        HashSet<string> setAllFilePath = new HashSet<string>();
        collectFilePath("Assets/", setAllFilePath);
        foreach (string path in setAllFilePath)
        {
            AssetImporter assetImporter = AssetImporter.GetAtPath(path);
            if (assetImporter != null)
            {
                assetImporter.assetBundleName = "";
            }
        }
        Debug.Log("******************************Reset finished");
    }
    private static void collectFilePath(string dir, HashSet<string> setPath)
    {
        foreach (string filePath in Directory.GetFiles(dir))
        {
            string lowerFilePath = filePath.ToLower();
            if (lowerFilePath.EndsWith(".cs"))
                continue;
            setPath.Add(filePath);
        }
        foreach (string directory in Directory.GetDirectories(dir))
        {
            collectFilePath(directory, setPath);
        }
    }
    public static string getPathRelative(string fullpath, string pattern)
    {
        //fullpath = fullpath.Replace("\\", "/");
        //fullpath = fullpath.ToLower();
        int resourceIndex = fullpath.IndexOf(pattern + "/");
        if (resourceIndex == -1)
        {
            resourceIndex = fullpath.IndexOf(pattern + "\\");
        }
        int patternLength = pattern.Length + 1;
        return fullpath.Substring(resourceIndex + patternLength, fullpath.Length - resourceIndex - patternLength);
    }

    public static string getPathRelativeWithPattern(string fullpath, string pattern)
    {
        return pattern + "/" + getPathRelative(fullpath, pattern);
    }
    //static private void setAssetBundleNameByDirectory(string path)
    //{
    //    int resourceIndex = path.IndexOf("Resources/");
    //    string pathName = path.Substring(resourceIndex + 10, path.Length - resourceIndex - 10);
    //    foreach (string filePath in Directory.GetFiles(path))
    //    {
    //        string lowerFilePath = filePath.ToLower();
    //        if (filePath.EndsWith(".meta"))
    //        {
    //            continue;
    //        }

    //        string[] depFilePaths = AssetDatabase.GetDependencies(filePath, false);
    //        foreach (string depFilePath in depFilePaths)
    //        {
    //            string lowerDepFilePath = depFilePath.ToLower();
    //            if (lowerDepFilePath.EndsWith(".cs"))
    //                continue;
    //            Debug.Log(filePath + "     " + depFilePath);
    //        }
    //        AssetImporter assetImporter = AssetImporter.GetAtPath(lowerFilePath);
    //        assetImporter.assetBundleName = pathName;
    //        assetImporter.assetBundleVariant = assetBundleVariant;
    //    }
    //    foreach (string directory in Directory.GetDirectories(path))
    //    {
    //        setAssetBundleNameByDirectory(directory);
    //    }
    //}
}
