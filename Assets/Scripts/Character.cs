using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Character
{
    public PlayerController playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();

    public List<Spell> spellbook;
    public float health;
    public float GCD = 0;
    private float maxGCD = 2;
    private float out_additive;
    private float out_multiplier;
    private float in_additive;
    private float in_multiplier;

    private Text textBox;
    public Animator anim;

    /// <summary>
    /// Constructor for Character class.
    /// </summary>
    /// <param name="spellbook"></param>
    /// <param name="textBox"></param>
    /// <param name="anim"></param>
    /// <param name="health"></param>
    public Character(List<Spell> spellbook, Text textBox, Animator anim, float health = 100)
    {
        this.health = health;
        this.spellbook = spellbook;
        this.textBox = textBox;
        this.anim = anim;
    }

    public void Update()
    {
        ReduceCooldowns();
    }

    /// <summary>
    /// Reduces GCD and cooldowns of every spell in the spellbook
    /// </summary>
    public void ReduceCooldowns()
    {
        //Reduces GCD
        GCD = Math.Max(GCD - Time.deltaTime, 0);

        //Reduces Spell CDs
        spellbook.ForEach(spell => spell.ReduceCooldown());
    }

    /// <summary>
    /// Sets the max GCD.
    /// </summary>
    /// <param name="newMaxGCD"></param>
    public void SetMaxGCD(float newMaxGCD)
    {
        this.maxGCD = newMaxGCD;
    }

    /// <summary>
    /// Get damage the character should deal given a base damage.
    /// </summary>
    /// <param name="baseDamage"></param>
    /// <returns></returns>
    public float GetDamage(float baseDamage)
    {
        return out_additive + out_multiplier * baseDamage; //Need to decide on order
    }

    /// <summary>
    /// Get damage the character should deal given a base damage.
    /// </summary>
    /// <param name="baseDamage"></param>
    public void TakeDamage(float baseDamage)
    {
        health -= in_additive + in_multiplier + baseDamage;
        textBox.text = Utils.ToDisplayText(health);
    }

    /// <summary>
    /// 
    /// </summary>
    public void TriggerGCD()
    {
        GCD = maxGCD;
    }

}



