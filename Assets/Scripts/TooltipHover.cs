using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipHover : MonoBehaviour {

    public Spell spell;

    private PlayerController playerController;
    private IEnumerator coroutine;
    private Transform instance;
    private List<Transform> instances = new List<Transform> { };

    public void Start()
    {
     playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
    }

    public void OnMouseEnter()
    {
        coroutine = TooltipBox();
        StartCoroutine(coroutine);

        //Instantiate Enemy Health Bar
        instance = MonoBehaviour.Instantiate((Transform)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Tooltip_BG.prefab", typeof(Transform)), new Vector3(0, 0, 0f), Quaternion.identity);
        instances.Add(instance);


        //Instantiate Text
        instance = MonoBehaviour.Instantiate((Transform)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/enemy_text.prefab", typeof(Transform)), new Vector3(0, 0, 0f), Quaternion.identity);
        TextMesh textBox = instance.GetComponent<TextMesh>();


        textBox.text = "Has " + spell.baseCooldown + " base cooldown";
        instances.Add(instance);
    }

    public void OnMouseOver()
    {
        Vector2 newPos = Input.mousePosition;
        newPos.x = newPos.x / 100 -4.65f;
        newPos.y = newPos.y / 100  -2.35f;
        instances.ForEach(instance=>instance.position = newPos);

    }

    public void OnMouseExit()
    {
        instances.ForEach(instance => Destroy(instance.gameObject));
        instances = new List<Transform> { };
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
