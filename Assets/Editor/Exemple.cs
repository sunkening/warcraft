using UnityEngine;
using UnityEditor;
using System.Collections;

public class Exemple : MonoBehaviour
{

    [MenuItem("Exemple/DisplayDialog", false, 0)]
    static void AddEasyTouch()
    {
        EditorUtility.DisplayDialog("Warning", "这个是弹窗", "OK");
        // Navigator
        //if (GameObject.FindObjectOfType<Navigator>() == null)
        //{
        //    new GameObject("Navigator", typeof(Navigator));
        //}
        //else
        //{
        //    EditorUtility.DisplayDialog("Warning", "Navigator is already exist in your scene", "OK");
        //}

        //Selection.activeObject = GameObject.FindObjectOfType<Navigator>().gameObject;
    }
    [MenuItem("Exemple/测试窗口")]
    static void OpenWindow()
    {
        EditorWindow.GetWindow<Demo_EditorWindow>(false, "窗口名", true).Show();
    }
}

public class Demo_EditorWindow : EditorWindow
{
    int toolBar = 0;
    void OnGUI()
    {
        EditorGUILayout.LabelField("Hello World!");
        //添加树向的背景方框
        EditorGUILayout.BeginVertical("Box");
        //这中间添加显示的内容
        EditorGUILayout.LabelField("内容1");
        //一个按扭
        GUILayout.Button(EditorGUIUtility.ObjectContent(null, typeof(Rigidbody)),
            GUILayout.Height(40), GUILayout.Width(200));
        EditorGUILayout.LabelField("内容2");
        EditorGUILayout.EndVertical();
        //求签页

        GUILayout.Toolbar(toolBar,
            new GUIContent[]
        {
            new
GUIContent("麻美由真"),
            new
GUIContent("苍井空"),
            new
GUIContent("波多野结衣"),
        });
        //横向排列
        EditorGUILayout.BeginHorizontal();
        switch
(toolBar)
        {
            case
0:
                EditorGUILayout.LabelField("textM");
                break;
            case
1:
                //图片显示
                //  GUILayout.Label((Texture2D)AssetDatabase.LoadAssetAtPath("Assets/demo001.jpg", typeof(Texture2D)));
                EditorGUILayout.TextArea("textCJK",
GUILayout.Width(260),
GUILayout.ExpandHeight(true));
                break;
            case
2:
                EditorGUILayout.LabelField("textB");
                break;
        }





        EditorGUILayout.EndHorizontal();





        //提示


        EditorGUILayout.HelpBox("提示内容",
MessageType.Info);





        //设置 key 与 value 之间，key的显示长度


        EditorGUIUtility.labelWidth
=
200;





        //缩进，数值越大，缩进越前 ,默认的缩进是0


        EditorGUI.indentLevel = 1;





        //        //开关


        //        isEditor = EditorGUILayout.Toggle("开关", isEditor);

        //        //禁用编辑
        //        EditorGUI.BeginDisabledGroup(isEditor);

        //        intNum = EditorGUILayout.IntField("编辑值", intNum);
        //        EditorGUI.EndDisabledGroup();
        //        //当编辑器改变时一定要调用此函数，此函数保存你所有的改变，此参数的作用域只有一个方法内。target 为改变的脚本
        //        if
        //(GUI.changed)


        //            EditorUtility.SetDirty(target);
    }
}