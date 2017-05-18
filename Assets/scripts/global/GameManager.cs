using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
using cfg;
public class GameManager : MonoBehaviour {
    public bool inited;
    public float progress;
    public int x;
    public int y;
    public int id;
    public CameraCotroller cameraCotroller;
    public static UnitManager unitManager = new UnitManager();
    public SpriteRenderer spriteRenderer;
    void Awake()
    {
        ResourceLoader.init();
        print("ResourceLoader finish");
        Global.Instance.Init();
        print("Global finish");
    }

    
    // Use this for initialization
    IEnumerator Start ()
    {
        yield return CfgLoader.Load();
        print("CfgLoader finish");
        yield return Map.Instance.load();
        print("map finish");
        Map.Instance.createMap(32,32,1);
        cameraCotroller=Camera.main.transform.parent.GetComponent<CameraCotroller>();
        cameraCotroller.init(Map.Instance);
        SpriteAnimCfg sfg=SpriteAnimCfg.get(1);
        Object o=Resources.Load("characters/1.0");
        print(o);
        Sprite s = Sprite.Create(o as Texture2D,new Rect(0,0,50,50),new Vector2());
        spriteRenderer.sprite = s;
        Resources.UnloadAsset(o);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnGUI()
    {
        if (GUILayout.Button("Press Me"))
            Map.Instance.setTile(x,y,id);
    }
}
