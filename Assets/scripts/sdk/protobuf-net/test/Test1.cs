using UnityEngine;
using System.Collections;
using ProtoBuf;
using System;
using System.Collections.Generic;


[ProtoContract]
public class Test1
{


    [ProtoMember(1)]
    public int Id
    {
        get;
        set;
    }


    [ProtoMember(2)]
    public List<String> data
    {
        get;
        set;
    }


    public override string ToString()
    {
        String str = Id + ":";
        foreach (String d in data)
        {
            str += d + ",";
        }
        return str;
    }

}