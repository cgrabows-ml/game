using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DummySummoner : Enemy
{

    public DummySummoner()
        : base("dummy summoner", "warrior", 3, maxGCD: 3)
    {
        GCD = 0f;
        this.isFixed = true;
        this.moveTo = new Vector2(5, -2.58f);
    }

    protected override List<Spell> getSpells()
    {
        Spell spell1 = new SummonDummy(this);
        List<Spell> spells = new List<Spell> { spell1 };
        return spells;
    }
}
