using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnergyHeal : Spell
{

    Hero hero;
    private int healingPerEnergy = 1;

    public EnergyHeal(Character caster)
        : base(caster, baseCooldown: 0, animationKey: "Use1",
            triggersGCD: true, GCDRespect: true, delay: 0)
    {
        hero = (Hero)caster;
    }

    public override bool isCastable()
    {
        Hero hero = (Hero)caster;
        return base.isCastable() && hero.GetEnergy() > 0;
    }

    public override void Cast()
    {
        base.Cast();
        int energy = hero.GetEnergy();
        int healing = healingPerEnergy * energy;
        hero.LoseEnergy(energy);
        hero.TakeDamage(-healing, caster);
    }
}
