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
        return null; ;
    }

    public override void Update()
    {

    }
}
