using UnityEngine;
using System.Collections;
using System;

public class PrintTTS : ITTS
{
    public void speak(string text)
    {
        Debug.Log(text);
    }
    public void setSpeed(int speed)
    {
         
    }
}
