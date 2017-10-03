using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPosisiotn : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    //gameObject.transform.localScale = Camera.main.(new Vector3(1.0f, 1.0f, 0f));
	}
	
	// Update is called once per frame
	void Update () {
        //1408*792
        Debug.Log("左下"+Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 2f)));
        Debug.Log("右上"+Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 2f)));
        //gameObject.transform.position = Camera.main.transform.forward * 2f;
        //gameObject.transform.localRotation = Quaternion.LookRotation(gameObject.transform.position);
	    //var pos = Camera.main.ScreenToWorldPoint(new Vector3(1408f, 792f, 2f));
        //pos = new Vector3(pos.x,pos.y,1f);
	    //gameObject.transform.localScale = pos;
	}
}
