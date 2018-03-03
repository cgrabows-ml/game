using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Spell
{
    public PlayerController playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();

    public String animationKey;
    protected float baseCooldown;
    protected float cooldown = 0;
    public float baseDamage;
    public Boolean triggersGCD;
    public Boolean GCDRespect;
    protected String target;

    public Spell(float baseCooldown, float baseDamage, String animationKey, Boolean triggersGCD = true, String target = "front", Boolean GCDRespect = true)
    {
        this.baseCooldown = baseCooldown;
        this.baseDamage = baseDamage;
        this.triggersGCD = triggersGCD;
        this.GCDRespect = true;
        this.animationKey = animationKey;
        this.target = target;
        this.GCDRespect = GCDRespect;
    }

    public virtual void Special(Character owner)
    {

    }

    public float GetCooldown()
    {
        return cooldown;
    }

    /// <summary>
    /// Reduces the cooldown of the ability, makes sure it's not below 0.
    /// </summary>
    public void ReduceCooldown()
    {
        cooldown = Math.Max(0, cooldown - Time.deltaTime);
    }

    /*/// <summary>
    /// Deals damage by changing variables in playercontroller.
    /// Uses the target variable to determine where to deal the damage.
    /// </summary>
    public void DealDamage(float damage)
    {
        List<Character> targets = GetTargets();
        targets.ForEach(target => target.TakeDamage(damage));
    }*/

    public List<Character> GetTargets()
    {
        if (target == "player")
        {
            return new List<Character>() { playerController.hero };
        }
        else if (target == "front")
        {
            return new List<Character>() { playerController.enemies[0] };
        }
        else if (target == "AoE")
        {
            List<Character> enemies = new List<Character>();
            foreach(Enemy enemy in playerController.enemies)
            {
                enemies.Add(enemy);
            }
            return enemies;
        }
        else if (target == "back")
        {
            return new List<Character>() { playerController.enemies[playerController.enemies.Count -1] };
        }
        else
        {
            return new List<Character>(); //Should instead raise error
        }
    }

    /// <summary>
    /// Used for hero casts only.  Returns if you can cast the spell.
    /// If the spell is not on CD and the GCD is not on CD, this returns true.
    /// Or if the spell is not on CD and the spell ignores the GCD, this returns true.
    /// Otherwise this returns false.
    /// </summary>
    /// <returns></returns>
    /*public Boolean CanCast()
    {
        return cooldown <= 0 && (owner.GCD <= 0 || GCDRespect == false);
    }*/

    /// <summary>
    /// Casts the spell.  Sets the cooldown to the base cooldown.  Deals damage.  If it doesnt ignore the GCD, then trigger the GCD.  If an ememy didn't cast this, reset multiplier and animate the hero.
    /// </summary>
    public virtual void Cast(Character caster)
    {
        cooldown = baseCooldown;
        if (triggersGCD)
        {
            caster.GCD = caster.maxGCD;
        }
        //DealDamage(caster.GetDamage(baseDamage));
        caster.anim.SetBool(animationKey, true);
    }
}
