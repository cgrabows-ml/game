using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeypressDownBig : MonoBehaviour {

    public KeyCode k;

	// Use this for initialization
	void Start () {
		
	}
	

	void Update () {

        if(Input.GetKeyDown(k))
        {
            this.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0f);
        }

        if (Input.GetKeyUp(k))
        {
            this.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
        }

    }
}
