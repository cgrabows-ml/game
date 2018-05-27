using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dummy : Enemy
{

    public Dummy()
        : base("warrior", "Warrior", health: 4, maxGCD: 100000)
    {
        GCD = 1000000;
    }

    protected override List<Spell> getSpells()
    {
        return new List<Spell> { }; ;
    }

    public override void Spawn(Vector2 pos)
    {
        base.Spawn(pos);
        this.isFixed = true;
    }

    public override float TakeDamage(float baseDamage, Character source)
    {
        float x = base.TakeDamage(baseDamage, source);

        return x;
    }
}
