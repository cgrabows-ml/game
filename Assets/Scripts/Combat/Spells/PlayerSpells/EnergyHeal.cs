using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnergyHeal : Spell
{

    private int healingPerEnergy = 1;

    public EnergyHeal()
        : base(baseCooldown: 0, animationKey: "Use1",
            triggersGCD: true, GCDRespect: true, delay: 0)
    {

    }

    public override bool isCastable(Character caster)
    {
        Hero hero = (Hero)caster;
        return base.isCastable(caster) && hero.GetEnergy() > 0;
    }

    public override void Cast(Character owner)
    {
        base.Cast(owner);
        Hero hero = (Hero)owner;
        int energy = hero.GetEnergy();
        int healing = healingPerEnergy * energy;
        hero.LoseEnergy(energy);
        hero.TakeDamage(-healing, owner);
    }
}
