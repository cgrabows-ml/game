using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Character : GameLogger
{
    public PlayerController playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();

    public List<Spell> spellbook;
    private float maxHealth;
    public float health;
    public float GCD = 0;
    public float maxGCD = 2;
    public float inAdditive = 0;
    public float outAdditive = 0;
    public float inMultiplier = 1;
    public float outMultiplier = 1;
    protected Text textBox;
    public Animator anim;
    public List<Buff> buffs = new List<Buff> { };
    private List<SpellCastObserver> spellCastObservers = new List<SpellCastObserver>();

    /// <summary>
    /// Constructor for Character class.
    /// </summary>
    /// <param name="spellbook"></param>
    /// <param name="textBox"></param>
    /// <param name="anim"></param>
    /// <param name="max_health"></param>
    public Character(List<Spell> spellbook, Text textBox, Animator anim,  float maxHealth = 100)
    {
        this.maxHealth = maxHealth;
        this.health = maxHealth;
        this.spellbook = spellbook;
        this.textBox = textBox;
        this.anim = anim;
}

    //Also casts the spell
    public Boolean CastIfAble(Spell spell)
    {
        if (!spellbook.Contains(spell))
        {
            //Throw error
        }
        Boolean castable =  (spell.GetCooldown() <= 0 && (GCD <= 0 || spell.GCDRespect == false));
        if (castable)
        {
            spell.Cast(this);
            spellCastObservers.ForEach(observer => observer.SpellCastUpdate(spell, this));
        }
        
        return castable;
    }

    /// <summary>
    /// Register object that will listen for spellcasts.
    /// </summary>
    /// <param name="observer"></param>
    public void RegisterCastListener(SpellCastObserver observer)
    {
        spellCastObservers.Add(observer);
    }

    public void UnregisterCastListener(SpellCastObserver observer)
    {
        spellCastObservers.Remove(observer);
    }

    //Right now this removes buffs every update... should be changed
    public void Update()
    {
        ReduceCooldowns();
        textBox.text = Utils.ToDisplayText(health);
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
        return outAdditive + (outMultiplier * baseDamage);
    }

    /// <summary>
    /// Takes damage given a base damage.
    /// </summary>
    /// <param name="baseDamage"></param>
    public void TakeDamage(float baseDamage)
    {
        health -= inAdditive + (inMultiplier * baseDamage);
    }

    /// <summary>
    /// 
    /// </summary>
    public void TriggerGCD()
    {
        GCD = maxGCD;
    }

}



