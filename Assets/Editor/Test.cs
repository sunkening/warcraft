using UnityEngine;
using System.Collections;
using UnityEditor;
public class Test   {
    
    [MenuItem("GameObject/Audio/Audio Room")]
    public static void CreateAudioRoom()
    {
        //GameObject room = new GameObject("Audio Room");
        //room.AddComponent<AudioRoom>();
        GameObject room = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //room.transform.parent = go.transform;
        room.GetComponent<Collider>().isTrigger = true;
        room.GetComponent<MeshRenderer>().enabled = false;
        room.transform.position = new Vector3(0, 1, 0);
        room.transform.localScale = new Vector3(10, 2, 5);
        room.name = "Audio Room";
        //room.AddComponent<AudioRoom>();
        GameObject portal = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ///GameObject portal = new GameObject("portal");
        //AudioPortal p = portal.AddComponent<AudioPortal>();
        portal.GetComponent<MeshRenderer>().enabled = false;
        portal.GetComponent<Collider>().isTrigger = true;
        //p.isOpen = true;
        //p.isShowLine = true;
        portal.transform.parent = room.transform;
        portal.transform.position = new Vector3(0, 0, -2.5f);
        //p.radius = 2;
        portal.name = "poratal";

    }

}
