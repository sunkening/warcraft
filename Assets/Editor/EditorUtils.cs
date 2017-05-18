using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class EditorUtils : MonoBehaviour
{
    public static T[] GetAssetsAtPath<T>(string path)
    {
        ArrayList list = new ArrayList();
        getAssetsAtPath<T>(path, list);
        T[] result = new T[list.Count];
        for (int i = 0; i < list.Count; i++)
            result[i] = (T)list[i];
        return result;
    }

    static void getAssetsAtPath<T>(string path, ArrayList list)
    {
        if (path[0] == '/')
            path = path.Substring(1);
        if (path[path.Length - 1] == '/')
            path = path.Substring(0, path.Length - 1);
        string strRootPath = Application.dataPath.Replace("Assets", "");
        string[] fileEntries = Directory.GetFiles(strRootPath + path + "/");
        foreach (string fileName in fileEntries)
        {
            int index = fileName.LastIndexOf("/");
            string localPath = path;

            if (index > 0)
                localPath += fileName.Substring(index);

            Object t = AssetDatabase.LoadAssetAtPath(localPath, typeof(T));

            if (t != null)
                list.Add(t);
        }

        string[] folderEntries = Directory.GetDirectories(strRootPath + "/" + path + "/");
        foreach (string folderName in folderEntries)
        {
            string strRelPath = folderName.Replace(strRootPath, "");
            getAssetsAtPath<T>(strRelPath, list);
        }
    }

    [MenuItem("Utils/Align/Align X (EditorUtils.cs)")]
    static public void AlignX()
    {
        if (!Selection.activeGameObject)
            return;
        if (Selection.gameObjects.Length <= 1)
            return;
        float x = Selection.activeGameObject.transform.position.x;
        foreach (GameObject obj in Selection.gameObjects)
        {
            Undo.RegisterUndo(obj.transform, "Align X");
            Vector3 position = obj.transform.position;
            position.x = x;
            obj.transform.position = position;
        }
    }

    [MenuItem("Utils/Align/Align Y (EditorUtils.cs)")]
    static public void AlignY()
    {
        if (!Selection.activeGameObject)
            return;
        if (Selection.gameObjects.Length <= 1)
            return;
        float y = Selection.activeGameObject.transform.position.y;
        foreach (GameObject obj in Selection.gameObjects)
        {
            Undo.RegisterUndo(obj.transform, "Align Y");
            Vector3 position = obj.transform.position;
            position.y = y;
            obj.transform.position = position;
        }
    }

    [MenuItem("Utils/Align/Align Z (EditorUtils.cs)")]
    static public void AlignZ()
    {
        if (!Selection.activeGameObject)
            return;
        if (Selection.gameObjects.Length <= 1)
            return;
        float z = Selection.activeGameObject.transform.position.z;
        foreach (GameObject obj in Selection.gameObjects)
        {
            Undo.RegisterUndo(obj.transform, "Align Z");
            Vector3 position = obj.transform.position;
            position.z = z;
            obj.transform.position = position;
        }
    }

    static public void AutoSwitchPlatform()
    {
        if(Application.platform == RuntimePlatform.WindowsEditor)
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
        if (Application.platform == RuntimePlatform.OSXEditor)
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.iOS);
    }

    [MenuItem("Utils/删除场景没用的MeshCollider和Animation (EditorUtils.cs)")]
    static public void Remove()
    {
        //获取当前场景里的所有游戏对象
        GameObject[] rootObjects = (GameObject[])UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
        //遍历游戏对象
        foreach (GameObject go in rootObjects)
        {
            //如果发现Render的shader是Diffuse并且颜色是白色，那么将它的shader修改成Mobile/Diffuse
            if (go != null && go.transform.parent != null)
            {
                Renderer render = go.GetComponent<Renderer>();
                if (render != null && render.sharedMaterial != null && render.sharedMaterial.shader.name == "Diffuse" && render.sharedMaterial.color == Color.white)
                {
                    render.sharedMaterial.shader = Shader.Find("Mobile/Diffuse");
                }
            }

            //删除所有的MeshCollider
            foreach (MeshCollider collider in UnityEngine.Object.FindObjectsOfType(typeof(MeshCollider)))
            {
                DestroyImmediate(collider);
            }

            //删除没有用的动画组件
            foreach (Animation animation in UnityEngine.Object.FindObjectsOfType(typeof(Animation)))
            {
                if (animation.clip == null)
                    DestroyImmediate(animation);
            }

            //应该没有人用Animator吧？ 避免美术弄错我都全部删除了。
            foreach (Animator animator in UnityEngine.Object.FindObjectsOfType(typeof(Animator)))
            {
                DestroyImmediate(animator);
            }
        }
        //保存
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Utils/批量删除所有场景中的MeshCollider 和Animation (EditorUtils.cs)")]
    static public void RemoveAll()
    {
        //遍历所有场景
        foreach (UnityEditor.EditorBuildSettingsScene scene in UnityEditor.EditorBuildSettings.scenes)
        {
            //当场景启动中
            if (scene.enabled)
            {
                //打开这个场景
                EditorApplication.OpenScene(scene.path);
                //删除该场景中的所有MeshCollider 和Animation
                Remove();
            }
        }
        //保存
        EditorApplication.SaveScene();
    }
}
