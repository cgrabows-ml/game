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
    public float cooldown = 0;
    public Boolean triggersGCD;
    public Boolean GCDRespect;
    protected String target;
    protected float delay;
    public int numEncounter;
    protected Character caster;
    public String name;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="baseCooldown"></param>
    /// <param name="animationKey"></param>
    /// <param name="triggersGCD"></param>
    /// <param name="target"></param>
    /// <param name="GCDRespect"></param>
    /// <param name="delay"></param>
    public Spell(Character caster,
        float baseCooldown, String animationKey, Boolean triggersGCD = true,
        Boolean GCDRespect = true, float delay = 0)
    {
        this.caster = caster;
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

    public virtual void Cast()
    {
        cooldown = baseCooldown;
        if (triggersGCD)
        {
            caster.GCD = caster.maxGCD;
        }
        //caster.anim.SetBool(animationKey, true);
    }

    public virtual Boolean isCastable()
    {
        return cooldown <= 0 && (caster.GCD <= 0 || GCDRespect == false);
    }

    IEnumerator CastAfterTime(float time)
    {
        float startTime = 0;
        while (startTime < time)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        Cast();
    }
}
