using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipHover : MonoBehaviour {

    void OnMouseOver()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        print("Mouse is over GameObject.");
    }

    void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
        print("Mouse is no longer on GameObject.");
    }
}
