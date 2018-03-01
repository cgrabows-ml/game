using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Spell
{

    public PlayerController playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();

    private float baseCooldown;
    private float cooldown = 0;
    private float damage;
    private Boolean triggersGCD;
    private float multiplier = 1;
    private Boolean GCDRespect;
    private String animationKey;

    public Spell(float baseCooldown, float damage, String animationKey, Boolean triggersGCD = true)
    {
        this.baseCooldown = baseCooldown;
        this.damage = damage;
        this.triggersGCD = triggersGCD;
        this.GCDRespect = true;
        this.animationKey = animationKey;
    }

    public void setMultiplier(float multiplier)
    {
        this.multiplier = multiplier;
    }

    // Use this for initialization
    void Start()
    {
    }

	void Update () {
        // reduceCooldown(); //TODO: This doesn't work. Logic is instead still in player controller
    }

    //Returns the value of the upper limit of the cooldown.  Used to display on screen.
    public string displayCD()
    {
        return (Math.Ceiling(cooldown)).ToString();
    }

    public float getCooldown()
    {
        return cooldown;
    }

    //Reduces the cooldown of the ability, makes sure it's not below 0
    public void reduceCooldown()
    {
        cooldown = Math.Max(0, cooldown - Time.deltaTime);
    }

    //Returns if the ability triggers the Global cooldown
    public Boolean getTriggersGCD()
    {
        return triggersGCD;
    }

    public void setGCDRespect(Boolean GCDRespect)
    {
        this.GCDRespect = GCDRespect;
    }

    //returns the damage of the spell after bonuses
    public void dealDamage()
    {
        playerController.enemyHealth -= damage * playerController.multiplier + playerController.additive;
    }

    public Boolean canCast()
    {
        return cooldown <= 0 && (playerController.GCD <= 0 || GCDRespect == false);
    }


    public void cast()
    {
        cooldown = baseCooldown;
        dealDamage();
        if (triggersGCD)
        {
            playerController.triggerGCD();
            triggersGCD = false;
        }
        playerController.hero.SetBool(animationKey, true);
        playerController.enemy.SetBool(animationKey, true);
        playerController.multiplier = multiplier;
    }
}
