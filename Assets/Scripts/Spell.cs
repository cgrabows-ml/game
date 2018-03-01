using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Spell
{

    public PlayerController playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
    public String animationKey;

    private float baseCooldown;
    private float cooldown = 0;
    private float damage;
    private Boolean triggersGCD;
    private float multiplier = 1;
    private Boolean GCDRespect;
    private String target;

    public Spell(float baseCooldown, float damage, String animationKey, Boolean triggersGCD = true, String target = "front")
    {
        this.baseCooldown = baseCooldown;
        this.damage = damage;
        this.triggersGCD = triggersGCD;
        this.GCDRespect = true;
        this.animationKey = animationKey;
        this.target = target;
    }

    /// <summary>
    /// Sets the multiplier in PlayerController to the parameter.
    /// </summary>
    /// <param name="multiplier"></param>
    public void setMultiplier(float multiplier)
    {
        this.multiplier = multiplier;
    }

    public float getCooldown()
    {
        return cooldown;
    }

    /// <summary>
    /// Reduces the cooldown of the ability, makes sure it's not below 0.
    /// </summary>
    public void reduceCooldown()
    {
        cooldown = Math.Max(0, cooldown - Time.deltaTime);
    }

    /// <summary>
    /// Returns if the ability triggers the Global cooldown
    /// </summary>
    /// <returns>triggersGCD</returns>
    public Boolean getTriggersGCD()
    {
        return triggersGCD;
    }

    /// <summary>
    /// Sets the spell to ignore the GCD if parameter is true.
    /// </summary>
    /// <param name="GCDRespect"></param>
    public void setGCDRespect(Boolean GCDRespect)
    {
        this.GCDRespect = GCDRespect;
    }

    /// <summary>
    /// Deals damage by changing variables in playercontroller.
    /// Uses the target variable to determine where to deal the damage.
    /// </summary>
    public void dealDamage()
    {
        if (target == "player")
        {
            playerController.heroHealth -= damage;
        }
        else if (target == "front")
        {
            playerController.enemies[0].health -= damage * playerController.multiplier + playerController.additive;
        }
        else if (target == "AoE")
        {
            foreach (Enemy enemy in playerController.enemies)
            {
                enemy.health -= damage * playerController.multiplier + playerController.additive;
            }
        }else if (target == "back")
        {
            playerController.enemies[playerController.enemies.Count - 1].health -= damage * playerController.multiplier + playerController.additive;
        }
    }

    /// <summary>
    /// Used for hero casts only.  Returns if you can cast the spell.
    /// If the spell is not on CD and the GCD is not on CD, this returns true.
    /// Or if the spell is not on CD and the spell ignores the GCD, this returns true.
    /// Otherwise this returns false.
    /// </summary>
    /// <returns></returns>
    public Boolean canCast()
    {
        return cooldown <= 0 && (playerController.GCD <= 0 || GCDRespect == false);
    }

    /// <summary>
    /// Returns if the enemy spell is on Cooldown or not.
    /// </summary>
    /// <returns></returns>
    public Boolean canEnemyCast()
    {
        return cooldown <= 0;
    }

    /// <summary>
    /// Casts the spell.  Sets the cooldown to the base cooldown.  Deals damage.  If it doesnt ignore the GCD, then trigger the GCD.  If an ememy didn't cast this, reset multiplier and animate the hero.
    /// </summary>
    public void cast()
    {
        cooldown = baseCooldown;
        dealDamage();
        if (triggersGCD && target != "player")
        {
            playerController.triggerGCD();
        }
        if (target != "player")
        {
            playerController.multiplier = multiplier;
            playerController.hero.SetBool(animationKey, true);
        }
    }
}
