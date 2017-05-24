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

    Unit unit = new Unit();
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
        //SpriteAnimCfg sfg=SpriteAnimCfg.get(1);
        //Object o=Resources.Load("characters/1.0");
        //print(o);
        //Sprite s = Sprite.Create(o as Texture2D,new Rect(0,0,50,50),new Vector2());
        //spriteRenderer.sprite = s;
        //Resources.UnloadAsset(o);
        //ResourceLoadTask task = new ResourceLoadTask();
        //task.path = "characters";
        //task.name = "1.0.png";
        //yield return ResourceLoader.LoadAssetAsync(task);
        //print(task.asset);
        yield return unitManager.init( );
        LoaderResult r = new LoaderResult();
        yield return unitManager.loadUnitType(1, r);
        
        GameObject d=GameObject.Instantiate(unitManager.characrerDrawerPrefab);
        SpriteDrawer drawer=d.AddComponent<SpriteDrawer>();
        Debug.Log (drawer);
        Debug.Log("************************");
        unit.init(unitManager.id2UnitType[1],drawer);
        unit.direction = UnitDirection.LookingNE;
        //foreach ( Sprite  l in unit.unitType.sprite.runAnim[1])
        //{
        //    Debug.Log(l);
        //}
         
    }
	
	// Update is called once per frame
	void Update () {
        if (unit.unitType!=null)
        {
            unit.draw();
        }
       
    }
    void OnGUI()
    {
        if (GUILayout.Button("Press Me"))
            Map.Instance.setTile(x,y,id);
    }
}
