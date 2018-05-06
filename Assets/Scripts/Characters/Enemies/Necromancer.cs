using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Necromancer : Enemy
{

    public Necromancer()
        : base("necromancer", "warrior", 5, maxGCD: 3)
    {
        this.isFixed = true;
        this.moveTo = new Vector2(5, -2.58f);
    }

    protected override List<Spell> getSpells()
    {
        Spell spell1 = new SummonSkeleton(this);
        Spell spell2 = new DamageSpell(this, 3, 4, "Use1", target:"player");
        List<Spell> spells = new List<Spell> { spell1, spell2 };
        return spells;
    }
}
