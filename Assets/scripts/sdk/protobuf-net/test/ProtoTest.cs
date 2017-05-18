using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ProtoBuf;
using System;
public class ProtoTest : MonoBehaviour
{
    private const String PATH = "c://data.bin";
    void Start()
    {
        //生成数据  
        List<Test1> testData = new List<Test1>();
        for (int i = 0; i < 100; i++)
        {
            testData.Add(new Test1() { Id = i, data = new List<string>(new string[] { "1", "2", "3" }) });
        }
        //将数据序列化后存入本地文件  
        using (Stream file = File.Create(PATH))
        {
            Serializer.Serialize<List<Test1>>(file, testData);
            file.Close();
        }
        //将数据从文件中读取出来，反序列化  
        List<Test1> fileData;
        using (Stream file = File.OpenRead(PATH))
        {
            fileData = Serializer.Deserialize<List<Test1>>(file);
        }
        //打印数据  
        foreach (Test1 data in fileData)
        {
            Debug.Log(data);
        }
    }


    // Update is called once per frame
    void Update()
    {
    }
}
