using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject o;
    public Sprite s;
	// Use this for initialization
	void Start () {
        //Astar a = new Astar();
        //float t1 = Time.realtimeSinceStartup;
        //for (int i = 0; i < 1; i++)
        //{
        //    a.init();
        //    a.findPath(0, 0, 99, 0);
        //}
        //float t2 = Time.realtimeSinceStartup;
        //print(a.total);
        //print(t2 - t1);
        //a.printMap();
        print(gameObject);
        Object  o1=Resources.Load("a");
        print(o1);
        Texture2D texture2D=o1 as Texture2D;
	    Sprite s=Sprite.Create(texture2D, new Rect(0, 0, 96, 96),new Vector2(0,0),96 );
        
        GetComponent<SpriteRenderer>().sprite = s;
	    for (int i = 0; i < 10; i++)
	    {
            print(MathUtil.GetIntRandomNumber(0, 12));
	    }
	}

    // Update is called once per frame
    void Update () {
        
    }
}
