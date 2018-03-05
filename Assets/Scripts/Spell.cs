using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Spell : GameLogger
{
    public PlayerController playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();

    public String animationKey;
    protected float baseCooldown;
    protected float cooldown = 0;
    public Boolean triggersGCD;
    public Boolean GCDRespect;
    protected String target;
    protected float delay;

    public Spell(float baseCooldown, String animationKey, Boolean triggersGCD = true, String target = "front", Boolean GCDRespect = true, float delay = 0)
    {
        this.baseCooldown = baseCooldown;
        this.triggersGCD = triggersGCD;
        this.GCDRespect = true;
        this.animationKey = animationKey;
        this.target = target;
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

    public virtual void Cast(Character caster)
    {
        cooldown = baseCooldown;
        if (triggersGCD)
        {
            caster.GCD = caster.maxGCD;
        }
        caster.anim.SetBool(animationKey, true);
    }
}
