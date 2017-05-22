using UnityEngine;
using System.Collections;

public class Global
{
    public const string TAG_STATICDOODAD = "staticDoodad";
    
    public const int LAYER_SINGPOSTSCANNED = 8;
    public const int LAYER_MASK_SINGPOST_SCANNED = 1 << LAYER_SINGPOSTSCANNED;
    public static bool showModel = true;
    public static Global Instance = new Global();
    public const int UNITY_EDITOR = 0;
    public const int UNITY_STANDALONE_WIN = 1;
    public const int UNITY_ANDROID = 2;
    public const int UNITY_IOS = 3;
    public static int PlatformType ;
    public static bool leidaBackground;
    public void Init()
    {
#if UNITY_EDITOR
        PlatformType = UNITY_EDITOR;
#elif UNITY_STANDALONE_WIN
       PlatformType=UNITY_STANDALONE_WIN;  
#elif UNITY_ANDROID
       PlatformType=UNITY_ANDROID;  
#elif UNITY_IOS
       PlatformType=UNITY_IOS;  
#endif
 
        //PlatformType = UNITY_ANDROID;
    }
}
