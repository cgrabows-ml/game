using System;
using System.Collections.Generic;
using UnityEngine;


public class Enemy
{

    private string name;
    private List<Spell> spellbook;
    private float health;
    private float castFreq;
    private float GCD = 2;

  /*  public Enemy(string name, float castFreq, List<Spell> spellbook, float health = 100)
    {
        this.name = name;
        this.health = health;
        this.castFreq = castFreq;
        this.spellbook = spellbook;
    }

    // Use this for initialization
    void Start()
    {

    }

    private void Update()
    {
        ReduceCDs();
        Cast();
    }

    //Reduces GCD and cooldowns of every spell in the spellbook
    private void ReduceCDs()
    {
        //Reduces GCD
        if (GCD > 0)
        {
            GCD -= Math.Max(Time.deltaTime, 0);
        }

        //Reduces Spell CDs
        for (int i = 0; i < spellbook.Count; i++)
        {
            spellbook[i].ReduceCooldown((float)(Time.deltaTime));
        }
    }

    private void Cast()
    {
        foreach(var spell in spellbook)
        {
            if (spell.CanCast(GCD))
            {
                spell.Cast();
                GCD = 2;
            }
        }
    }*/
    
}
	

