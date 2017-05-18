using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using cfg;

public class NewBehaviourScript1 : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        //方法1
        //在项目根目录中创建 Resources 文件夹来保存文件。
//         object o = Resources.Load("student");
//         print(o.GetType());
        //方法2
        //Application.dataPath=D:/unity/New Unity Project/Assets
        //移动端没有访问权限
        print("Application.dataPath=" + Application.dataPath);
        //方法3
        //在项目根目录中创建StreamingAssets文件夹来保存文件



        //方法4
        //推荐方式
        print("Application.persistentDataPath=" + Application.persistentDataPath);

        //方法5
        print("Application.temporaryCachePath=" + Application.temporaryCachePath);






       
    }

    // Update is called once per frame
    void Update()
    {

    }
}
