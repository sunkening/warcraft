using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCotroller : MonoBehaviour {
    public Camera maincamera;
    public float scrollSpeed = 50;
    public float moveSpeed = 10;
    public float cameraHeight;
    public float cameraSize;
    public float cameraMinSize = 2;
    public float cameraMaxSize = 20;
    // Use this for initialization
    void Start ()
    {
        scrollSpeed = 50;
        maincamera = Camera.main;
        maincamera.transform.localEulerAngles=new Vector3(45,0,0);
        transform.localEulerAngles = new Vector3(0, 45, 0);
    }
	
	// Update is called once per frame
	void Update ()
    {
       
        if (Input.mouseScrollDelta.y!=0)
        {
            scroll(-Input.mouseScrollDelta.y);
        }
	    if (Input.GetKey("a"))
	    {
            move(-Time.deltaTime, 0);
	    }
        if (Input.GetKey("d"))
        {
            move( Time.deltaTime, 0);
        }
        if (Input.GetKey("w"))
        {
            move(0, Time.deltaTime );
        }
        if (Input.GetKey("s"))
        {
            move(0, -  Time.deltaTime);
        }
    }

    public void move(float x,float z)
    {
        transform.Translate(new Vector3(x* moveSpeed, 0,z* moveSpeed),Space.Self);
    }
    public void scroll(float y)
    {
        maincamera.orthographicSize += y * Time.deltaTime * scrollSpeed;
        if (maincamera.orthographicSize<cameraMinSize)
        {
            maincamera.orthographicSize = cameraMinSize;
        }
        if (maincamera.orthographicSize > cameraMaxSize)
        {
            maincamera.orthographicSize = cameraMaxSize;
        }
    }
    public void init(Map map)
    {
        cameraHeight = map.widthInTiles/2/Mathf.Sin(45);
        transform.position=new Vector3(0, cameraHeight, 0);
    }
}
