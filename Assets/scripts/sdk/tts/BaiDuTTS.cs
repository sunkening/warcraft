using UnityEngine;
using System.Collections;
using System;

public class BaiDuTTS : ITTS {
    AndroidJavaObject jo;
    public BaiDuTTS()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
          jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        setSpeed(7);
        setVolume(3);
    }
    public void speak(string text)
    {
        jo.Call("stop");
        jo.Call("speak",text);
    }
    public void setSpeed(int speed)
    {
        jo.Call("setSpeed", speed);
    }
    public void setVolume(int v)
    {
        jo.Call("setVolume", v);
    }
}
