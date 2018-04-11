using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyGenerator : DamageSpell
{

    public EnergyGenerator(Character caster)
        : base(caster, baseCooldown: 1, baseDamage: 1, animationKey: "Use2",
            triggersGCD: true, target: "front", GCDRespect: true, delay:0)
    {

    }

    public override void Cast()
    {
        base.Cast();
        Hero hero = (Hero)caster;
        hero.GainEnergy(1);
    }
}
