using UnityEngine;
using System.Collections;
using UnityEditor;

[InitializeOnLoad]
public class EditorMain   {
 
    
    static EditorMain()
    {
        EditorApplication.update += Update;
        CfgLoader.LoadForEditor();
    }
    void Start () {
	
	}

    // Update is called once per frame
    static void Update () {
        //SignpostInspetor.Update();

    }
}
