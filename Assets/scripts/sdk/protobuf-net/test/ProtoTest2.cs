using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
 
using ProtoBuf;
using System;
 
using msg.login;
using msg;
using skntcp;
public class ProtoTest2 : MonoBehaviour
{
    private const String PATH = "data.bin";
    void Start()
    {
        MessageManager.instance.init();
        ProtobufTCPClient client = new ProtobufTCPClient();
        client.connect("127.0.0.1",9123,new ProtobufHandler());


    }


    // Update is called once per frame
    void Update()
    {
    }
}
