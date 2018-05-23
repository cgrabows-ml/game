using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpellBinding
{
    public GameController gameController = GameController.gameController;
    public Spell spell;
    private KeyCode key;
    private Text textBox;
    private RectTransform castCover;
    private Hero hero = GameController.gameController.hero;
    private Vector3 castCoverBasePosition;

	public SpellBinding(Spell spell, KeyCode key, Text textBox, RectTransform castCover) {
        this.castCover = castCover;
        this.spell = spell;
        this.key = key;
        this.textBox = textBox;
        this.castCoverBasePosition = castCover.localPosition;
	}

    public void Update()
    {
        UpdateTextBox();
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

        castCover.localScale = new Vector2(1, 1 - percentDone);
        castCover.localPosition = castCoverBasePosition -
            new Vector3(0, percentDone * castCover.rect.height/2);
    }

    public void UpdateTextBox()
    {
        this.textBox.text = Utils.ToDisplayText(spell.GetCooldown());
    }


    public KeyCode GetKey()
    {
        return key;
    }

}
