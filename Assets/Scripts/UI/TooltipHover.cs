using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipHover : MonoBehaviour {

    public Spell spell;

    //public GameController gameController;
    public GameObject tooltipBG;
    public GameObject tooltipText;

    private IEnumerator coroutine;
    private Transform instance;
    //private List<GameObject> tooltips = new List<GameObject> { };

    public void Start()
    {
        //gameController = GameObject.Find("GameController").GetComponent<gameController>();
    }

    public void OnMouseEnter()
    {
        coroutine = TooltipBox();
        StartCoroutine(coroutine);

        tooltipBG.gameObject.SetActive(true);
        tooltipText.gameObject.SetActive(true);



        //Instantiate Text
        Text textBox = tooltipText.GetComponent<Text>();


        textBox.text = "Has " + spell.baseCooldown + " base cooldown";
    }

    public void OnMouseOver()
    {

    }

    public void OnMouseExit()
    {
        tooltipBG.gameObject.SetActive(false);
        tooltipText.gameObject.SetActive(false);
    }

    public void Update()
    {
        if(Input.mousePosition == new Vector3(0, 0, 0))
        {
            DeleteTooltip();
        }
    }

    public void DeleteTooltip()
    {

    }

    IEnumerator TooltipBox()
    {

        yield return null;
    }

}
