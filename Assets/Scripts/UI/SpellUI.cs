using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpellUI
{
    public GameController gameController = GameController.gameController;
    public Spell spell;
    private KeyCode key;
    private Text textBox;
    private RectTransform castCover;
    private Hero hero = GameController.gameController.hero;
    private Vector3 castCoverBasePosition;
    private float scale = .01f;
    private Vector3 basePosition = new Vector3(-1.5f, 2f);

    private Transform castBox;

    public Transform spellUI;

	public SpellUI(Spell spell, KeyCode key, int position) {
        InstantiateSpellUI();
        this.spell = spell;
        this.key = key;
        SetOffset(position);
        //this.castCoverBasePosition = castCover.localPosition;
	}

    public void SetOffset(int position)
    {
        //float width = castBox.GetComponent<Image>().flexibleWidth;
        float width = 1f;
        //spellUI.position = new Vector3(0, 0, 0);
        spellUI.position += new Vector3(width, 0f)*position;
    }

    public void InstantiateSpellUI()
    {
        GameObject prefab = (GameObject)Resources.Load("SpellUI");
        spellUI = MonoBehaviour.Instantiate(prefab).transform;
        //textBox = spellUI.GetComponent<Text>("Cast 1 Text");
        spellUI.position = basePosition;
        spellUI.SetParent(gameController.canvas.transform);

        for (int i = 0; i < spellUI.transform.childCount; i++)
        {
            Transform child = spellUI.transform.GetChild(i).transform;
            child.localPosition = new Vector2(0, 0);
            //child.position = gameController.hero.sprite.position;
            child.transform.localScale = new Vector3(scale, scale);

            //if (child.name == "CastText")
            //{
            //    textBox = child.GetComponent<Text>();
            //}
            if(child.name == "CooldownCover")
            {
                castCover = child.GetComponent<RectTransform>();
            }
            if(child.name == "CastBox")
            {
                castBox = child;
            }
        }
    }

    public void Update()
    {
        //UpdateTextBox();
        UpdateCooldownCover();
    }

    private void UpdateCooldownCover()
    {
        float recentMax;
        float timeRemaining;
        float cooldown = spell.GetCooldown();
        float percentDone;
        if(cooldown < hero.GCD && spell.GCDRespect)
        {
            recentMax = hero.maxGCD;
            timeRemaining = hero.GCD;
        }
        else
        {
            recentMax = spell.recentMaxCD;
            timeRemaining = cooldown;
        }
        if (recentMax == 0)
        {
            percentDone = 1;
        }
        else
        {
            percentDone = 1 - timeRemaining / recentMax;
        }

        castCover.localScale = new Vector2(1, 1 - percentDone)* scale;
        castCover.localPosition = castCoverBasePosition -
            new Vector3(0, percentDone * castCover.rect.height/2)*scale;
    }

    //public void UpdateTextBox()
    //{
    //    this.textBox.text = Utils.ToDisplayText(spell.GetCooldown());
    //}


    public KeyCode GetKey()
    {
        return key;
    }

}
