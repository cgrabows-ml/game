using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeypressShrink : MonoBehaviour {

    public KeyCode k;

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(k))
        {
            this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(72, 72);
        }

        if (Input.GetKeyUp(k))
        {
            this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);
        }
    }
}
