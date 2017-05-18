using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maptest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    void OnTriggerEnter2D(Collider2D collider2D)
    {
        print(gameObject.name);
    }
}
