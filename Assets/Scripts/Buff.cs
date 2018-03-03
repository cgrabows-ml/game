using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff {

    public PlayerController playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>(); //USED FOR TESTING REMOVE THIS

    public float multiplier = 1;
    public float addititve = 0;
    public String removeWhen = "";
    public string applies = "";
    public String name = "";
    public float duration;
    public int numSpells;
    public int numHits;
    public List<String> castNames;


    public Buff(float multiplier, float addititve, String name, String removeWhen, int duration = 0, int numSpells = 0, int numHits = 0, List<String> castNames = null)
    {
        this.multiplier = multiplier;
        this.addititve = addititve;
        this.removeWhen = removeWhen;
        this.name = name;
        this.duration = duration;
        this.numSpells = numSpells;
        this.numHits = numHits;
        if (castNames == null)
        {
            castNames = new List<String> { };
        }
        else
        {
            this.castNames = castNames;
        }
    }

    //removeWhen examples: time, # of spells cast, on hit, certain spell/set of spells is cast
    //Implementing duration, numSpells, numHits, castNames
    //Implement bonuses, armor, DoT/HoT

    public Boolean CanRemove()
    {
        //Duration
        if (removeWhen == "duration")
        {
            if(duration <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //Number of Spells
        if (removeWhen == "numSpells")
        {
            if (numSpells <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Right now, the only time this else should get returned is if the removeWhen string is wrong, or if it's meant to be permanent.
        else
        {
            return false;
        }
    }

    public void RemoveBuff(Character owner)
    {
        owner.out_multiplier = 1;
    }

    /// <summary>
    /// Removes duration based on deltatime
    /// </summary>
    public void Update()
    {
        duration -= Math.Max(Time.deltaTime, 0);
        duration = Math.Max(duration, 0);
    }


}
