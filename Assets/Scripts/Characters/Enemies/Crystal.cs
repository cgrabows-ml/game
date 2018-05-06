using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crystal : Enemy
{

    public Crystal()
        : base("crystal", "warrior", health:15, maxGCD: 3)
    {
        isFixed = true;
        this.moveTo = new Vector2(3f, -2.58f);
    }

    public override float TakeDamage(float damage, Character source)
    {
        float healthBefore = health;
        float damageTaken =  base.TakeDamage(damage, source);
        if (health == 0 && healthBefore > 0)
        {
            CrystalBuff c = new CrystalBuff(source, sprite.localPosition);
            c.ApplyBuff();

        }
        return damageTaken;
    }

    protected override List<Spell> getSpells()
    {
        List<Spell> spells = new List<Spell> {};
        return spells;
    }
}
