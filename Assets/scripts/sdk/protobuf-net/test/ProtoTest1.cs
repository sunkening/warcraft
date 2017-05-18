using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
 
using ProtoBuf;
using System;
 
using msg.login;
using msg;
 
public class ProtoTest1 : MonoBehaviour
{
    private const String PATH = "data.bin";
    void Start()
    {
        CAccount a = new CAccount();
        a.name = "孙克宁";
        a.password = "123456";
        MsgHead msghead = new MsgHead();
        msghead.msgType =  (uint)MsgType.CAccount;
        MemoryStream memStream = new MemoryStream();
        Serializer.Serialize<CAccount>(memStream, a);
        byte[] messagebody = memStream.GetBuffer();
        msghead.msgLength = (uint)memStream.Length;
        MemoryStream memStreamHead = new MemoryStream();
        Serializer.Serialize<MsgHead>(memStreamHead, msghead);
        //将数据序列化后存入本地文件  
        using (Stream file = File.Create(PATH))
        {
            file.Write(memStreamHead.GetBuffer(),0,10);
             file.Write(messagebody, 0, messagebody.Length);
            file.Close();
        }
        //将数据从文件中读取出来，反序列化  
     
        using (FileStream file = File.OpenRead(PATH))
        {
            BinaryReader r = new BinaryReader(file);
            r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开  
            byte[] pReadByte = r.ReadBytes(10);
            MemoryStream msgh = new MemoryStream(pReadByte);
            MsgHead baseMsg = Serializer.Deserialize<MsgHead> (msgh);
            print(baseMsg.msgLength);
            print(baseMsg.msgType);
            r.BaseStream.Seek(10, SeekOrigin.Begin);    //将文件指针设置到文件开 
            pReadByte = r.ReadBytes((int)baseMsg.msgLength);
            MemoryStream msgboddy = new MemoryStream(pReadByte);
            CAccount ac = Serializer.Deserialize<CAccount>(msgboddy);
            print(ac.name);
            print(ac.password);
        }
       
    }


    // Update is called once per frame
    void Update()
    {
    }
}
