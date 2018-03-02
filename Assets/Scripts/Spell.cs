using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Spell
{
    public PlayerController playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();

    public String animationKey;

    private Character owner;
    private float baseCooldown;
    private float cooldown = 0;
    private float baseDamage;
    private Boolean triggersGCD;
    private float multiplier = 1;
    private Boolean GCDRespect;
    private String target;

    public Spell(Character owner, float baseCooldown, float baseDamage, String animationKey, Boolean triggersGCD = true, String target = "front")
    {
        this.owner = owner;
        this.baseCooldown = baseCooldown;
        this.baseDamage = baseDamage;
        this.triggersGCD = triggersGCD;
        this.GCDRespect = true;
        this.animationKey = animationKey;
        this.target = target;
    }

    /// <summary>
    /// Sets the multiplier in PlayerController to the parameter.
    /// </summary>
    /// <param name="multiplier"></param>
    public void SetMultiplier(float multiplier)
    {
        this.multiplier = multiplier;
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

    /// <summary>
    /// Sets the spell to ignore the GCD if parameter is true.
    /// </summary>
    /// <param name="GCDRespect"></param>
    public void SetGCDRespect(Boolean GCDRespect)
    {
        this.GCDRespect = GCDRespect;
    }

    /// <summary>
    /// Deals damage by changing variables in playercontroller.
    /// Uses the target variable to determine where to deal the damage.
    /// </summary>
    public void DealDamage()
    {
        float damage = owner.GetDamage(baseDamage);
        List<Character> targets = GetTargets();
        targets.ForEach(target => target.TakeDamage(damage));
    }

    private List<Character> GetTargets()
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
            return playerController.enemies;
        }
        else if (target == "back")
        {
            return new List<Character>() { playerController.enemies[-1] };
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
    public Boolean CanCast()
    {
        return cooldown <= 0 && (owner.GCD <= 0 || GCDRespect == false);
    }

    /// <summary>
    /// Returns if the enemy spell is on Cooldown or not.
    /// </summary>
    /// <returns></returns>
    public Boolean CanEnemyCast()
    {
        return cooldown <= 0;
    }

    /// <summary>
    /// Casts the spell.  Sets the cooldown to the base cooldown.  Deals damage.  If it doesnt ignore the GCD, then trigger the GCD.  If an ememy didn't cast this, reset multiplier and animate the hero.
    /// </summary>
    public void Cast()
    {
        cooldown = baseCooldown;
        DealDamage();
        if (triggersGCD && target != "player")
        {
            owner.TriggerGCD();
        }
        if (target != "player")
        {
            owner.out_multiplier = multiplier;
            owner.anim.SetBool(animationKey, true);
        }
    }
}
