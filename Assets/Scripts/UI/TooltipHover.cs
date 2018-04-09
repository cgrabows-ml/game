using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipHover : MonoBehaviour {

    public Spell spell;

    public GameObject tooltipBG;
    public GameObject tooltipText;

    public void Start()
    {

    }

    public void OnMouseEnter()
    {
        tooltipBG.gameObject.SetActive(true);
        tooltipText.gameObject.SetActive(true);



        //Instantiate Text
        Text textBox = tooltipText.GetComponent<Text>();


        textBox.text = "Has " + spell.baseCooldown + " base cooldown";
    }

    public void OnMouseExit()
    {
        tooltipBG.gameObject.SetActive(false);
        tooltipText.gameObject.SetActive(false);
    }

    public void Update()
    {
        
    }



}
