using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnergyGenerator : DamageSpell
{

    public EnergyGenerator()
        : base(baseCooldown: 1, baseDamage: 1, animationKey: "Use2",
            triggersGCD: true, target: "front", GCDRespect: true, delay:0)
    {

    }

    public override void Cast(Character owner)
    {
        base.Cast(owner);
        Hero hero = (Hero)owner;
        hero.GainEnergy(1);
    }
}
