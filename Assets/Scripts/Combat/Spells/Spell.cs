using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public abstract class Spell
{
    public GameController gameController =
        GameObject.Find("GameController").GetComponent<GameController>();

    public String animationKey;
    public float baseCooldown;
    protected float cooldown = 0;
    public Boolean triggersGCD;
    public Boolean GCDRespect;
    protected String target;
    protected float delay;
    public int numEncounter;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="baseCooldown"></param>
    /// <param name="animationKey"></param>
    /// <param name="triggersGCD"></param>
    /// <param name="target"></param>
    /// <param name="GCDRespect"></param>
    /// <param name="delay"></param>
    public Spell(float baseCooldown, String animationKey, Boolean triggersGCD = true,
        Boolean GCDRespect = true, float delay = 0)
    {
        this.baseCooldown = baseCooldown;
        this.triggersGCD = triggersGCD;
        this.GCDRespect = true;
        this.animationKey = animationKey;
        this.GCDRespect = GCDRespect;
        this.delay = delay;
    }

    public float GetCooldown()
    {
        return cooldown;
    }

    /// <summary>
    /// Reduces the cooldown of the ability, makes sure it's not below 0.
    /// </summary>
    public virtual void ReduceCooldown()
    {
        cooldown = Math.Max(0, cooldown - Time.deltaTime);
    }

    public virtual void Cast(Character caster)
    {
        cooldown = baseCooldown;
        if (triggersGCD)
        {
            caster.GCD = caster.maxGCD;
        }
        //caster.anim.SetBool(animationKey, true);
    }

    public virtual Boolean isCastable(Character caster)
    {
        return cooldown <= 0 && (caster.GCD <= 0 || GCDRespect == false);
    }

    IEnumerator DamageAfterTime(float time, Character owner)
    {
        float startTime = 0;
        while (startTime < time)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        Cast(owner);
    }
}
