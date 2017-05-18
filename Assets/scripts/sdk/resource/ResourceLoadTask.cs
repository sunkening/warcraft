using UnityEngine;
using System.Collections;

public class ResourceLoadTask   {
   // public bool finished = false;
    public int state=ResourceLoader.STATE_NOT_BEGIN;
    public string variant;
    public string path;
    public string name;
    public object asset;
    //private IEnumerator doWait()
    //{
    //    while (!finished)
    //    {
    //        yield return new WaitForEndOfFrame();
    //    }
    //}
}
