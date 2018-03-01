using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Enemy
{
    public PlayerController playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
    public List<Spell> spellbook;
    public float health;
    public float GCD = 2;

    private string name;
    private float castFreq;
    private Text textBox;
    private Animator anim;

    public Enemy(string name, float castFreq, List<Spell> spellbook, Text textBox, Animator anim, float health = 100)
    {
        this.name = name;
        this.health = health;
        this.castFreq = castFreq;
        this.spellbook = spellbook;
        this.textBox = textBox;
        this.anim = anim;
    }

    /// <summary>
    /// Updates the text box that was entered with the constructor.
    /// </summary>
    public void UpdateTextBox()
    {
        textBox.text = playerController.textConverter(health);
    }

    /// <summary>
    /// This is the AI for the enemy.  The enemy goes through his spellbook and sees if it's valid for him to cast each spell.  If so, he casts it.  This also triggers the enemy GCD, and animates the enemy.
    /// </summary>
    public void Cast()
    {
        foreach(Spell spell in spellbook)
        {
            if (spell.canEnemyCast() && GCD <= 0)
            { 
                spell.cast();
                anim.SetBool(spell.animationKey, true);
                GCD = castFreq;
            }
        }
    }

    /// <summary>
    /// Reduces GCD and cooldowns of every spell in the spellbook
    /// </summary>
    public void ReduceCooldowns()
    {
        //Reduces GCD
        if (GCD > 0)
        {
            GCD -= Math.Max(Time.deltaTime, 0);
        }

        //Reduces Spell CDs
        for (int i = 0; i < spellbook.Count; i++)
        {
            spellbook[i].reduceCooldown();
        }
    }
    
}
	

