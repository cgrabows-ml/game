using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Enemy : Character
{
    private string name;

    /// <summary>
    /// Constructor for enemy class
    /// </summary>
    /// <param name="name"></param>
    /// <param name="castFreq"></param>
    /// <param name="spellbook"></param>
    /// <param name="textBox"></param>
    /// <param name="anim"></param>
    /// <param name="health"></param>
    public Enemy(string name, float castFreq, List<Spell> spellbook, Text textBox, Animator anim, float health = 100)
        : base(spellbook, textBox, anim, health)
    {
        base.SetMaxGCD(castFreq);
        //maxGCD = castFreq; this should be possible
    }

    new public Update()
    {
        base.Update();
        Cast();
    }

    /// <summary>
    /// This is the AI for the enemy.  The enemy goes through his spellbook and sees if it's valid for him to cast each spell.  If so, he casts it.  This also triggers the enemy GCD, and animates the enemy.
    /// </summary>
    public void Cast()
    {
        foreach(Spell spell in spellbook)
        {
            if (spell.CanCast() && GCD <= 0)
            {
                spell.Cast();
                anim.SetBool(spell.animationKey, true);
                GCD = maxGCD;
            }
        }
    }
}
	

