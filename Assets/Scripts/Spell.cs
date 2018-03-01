using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Spell {

    private float baseCooldown;
    private float cooldown;
    private float damage;
    private Boolean triggersGCD;
    private float setMultiplier;
    private Boolean GCDRespect;

    public Spell(float newBaseCD, float newDamage, Boolean newTriggersGCD)
    {
        baseCooldown = newBaseCD;
        damage = newDamage;
        triggersGCD = newTriggersGCD;
        setMultiplier = 1;
        GCDRespect = true;
    }

    // Use this for initialization
    void Start()
    {
        cooldown = 0;
    }


	void Update () {
        ReduceCooldown(Time.deltaTime);
	}

    //Returns the value of the upper limit of the cooldown.  Used to display on screen.
    public string displayCD()
    {
        return (Math.Ceiling(cooldown)).ToString();
    }

    float GetCooldown()
    {
        return cooldown;
    }

    public void SetMultiplier(float multi)
    {
        setMultiplier = multi;
    }

    public float GetMultiplier()
    {
        return setMultiplier;
    }

    //Reduces the cooldown of the ability, makes sure it's not below 0
    public void ReduceCooldown(float difference)
    {
        if(cooldown > 0)
        {
            cooldown -= difference;
        }

        if(cooldown < 0)
        {
            cooldown = 0;
        }
    }

    //Returns if the ability triggers the Global cooldown
    public Boolean GetTriggersGCD()
    {
        return triggersGCD;
    }

    public void SetGCDRespectFalse()
    {
        GCDRespect = false;
    }

    //returns the damage of the spell after bonuses
    public float DealDamage(float multiplier, float additive)
    {
        return damage * multiplier + additive;
    }

    public Boolean CanCast(float GCD)
    {
        if (cooldown <= 0 && (GCD <= 0 || GCDRespect == false))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //sets the cooldown of the spell and returns damage
    public float Cast(float multiplier = 1, float addititve = 0)
    {
        cooldown = baseCooldown;
        return DealDamage(multiplier, addititve);

    }


}
