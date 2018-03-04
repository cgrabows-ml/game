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
    public float maxGCD = 2;
    public float in_additive = 0;
    public float out_additive = 0;
    public float in_multiplier = 1;
    public float out_multiplier = 1;
    public TextMesh textBox;
    public Animator anim;
    public List<Buff> buffs = new List<Buff> { };
    public List<Transform> instances;

    /// <summary>
    /// Constructor for Character class.
    /// </summary>
    /// <param name="spellbook"></param>
    /// <param name="textBox"></param>
    /// <param name="anim"></param>
    /// <param name="health"></param>
    public Character(List<Spell> spellbook, TextMesh textBox, Animator anim,  float health = 100)
    {
        this.health = health;
        this.spellbook = spellbook;
        this.textBox = textBox;
        this.anim = anim;
}

    //Also casts the spell TODO: Rename
    public Boolean CanCast(int i)
    {
        Boolean castable =  (spellbook[i].GetCooldown() <= 0 && (GCD <= 0 || spellbook[i].GCDRespect == false));
        if (castable)
        {
            spellbook[i].Cast(this);
            ReduceNumSpell();
        }
        
        return castable;
    }

    /// <summary>
    /// Reduces the number of spells variable in buffs
    /// </summary>
    public void ReduceNumSpell()
    {
        foreach (Buff buff in buffs)
        {
            buff.numSpells = Math.Max(buff.numSpells - 1, 0);
        }
    }

    //Right now this removes buffs every update... should be changed
    public void Update()
    {
        ReduceCooldowns();
        RemoveBuffs();
        textBox.text = Utils.ToDisplayText(Math.Max(health, 0));
    }

    public void RemoveBuffs()
    {
        List<Buff> toRemove = new List<Buff> { };
        foreach(Buff buff in buffs)
        {
            if (buff.CanRemove())
            {
                buff.RemoveBuff(this);
                toRemove.Add(buff);
            }
        }
        foreach(Buff buff in toRemove)
        {
            buffs.Remove(buff);
        }
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
    public void SetMaxGCD(float maxGCD)
    {
        this.maxGCD = maxGCD;
    }

    /// <summary>
    /// Get damage the character should deal given a base damage.
    /// </summary>
    /// <param name="baseDamage"></param>
    /// <returns></returns>
    public float GetDamage(float baseDamage)
    {
        return out_additive + (out_multiplier * baseDamage);
    }

    /// <summary>
    /// Takes damage given a base damage.
    /// </summary>
    /// <param name="baseDamage"></param>
    public void TakeDamage(float baseDamage)
    {
        health -= in_additive + in_multiplier * baseDamage;
    }

    /// <summary>
    /// 
    /// </summary>
    public void TriggerGCD()
    {
        GCD = maxGCD;
    }

}



