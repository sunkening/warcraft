using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    //public Sprite[] sprits;
    public List<SpriteRenderer>   spriteRenderers;
    public int tileCfgId;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void removeSprit()
    {
        foreach (SpriteRenderer s in spriteRenderers)
        {
            s.sprite = null;
        }
    }
}
